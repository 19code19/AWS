using Newtonsoft.Json.Linq;

class Program
{
    static void Main()
    {
        string jsonFilePath = "conditions.json";
        string json = File.ReadAllText(jsonFilePath);
        string targetName = "Tpp";

        var inputValues = new Dictionary<string, object>
        {
            { "type", "test" },
            { "paymenttype", 2 },
            { "city", 5 }
        };

        int? matchingRuleId = ExecuteAny(json, targetName, inputValues);
        if (matchingRuleId.HasValue)
        {
            Console.WriteLine("Match Found: Rule ID = " + matchingRuleId.Value);
        }
        else
        {
            Console.WriteLine("No Match Found");
        }

        var unmatchedRuleIds = ExecuteAll(json, targetName, inputValues);
        if (unmatchedRuleIds.Count != 0)
        {
            Console.WriteLine("Unmatched Rule IDs: " + string.Join(", ", unmatchedRuleIds));
        }
        else
        {
            Console.WriteLine("All rules matched");
        }
    }

    static int? ExecuteAny(string json, string targetName, Dictionary<string, object> inputValues)
    {
        var rulesArray = GetRulesArray(json, targetName);
        if (rulesArray != null)
        {
            foreach (var rule in rulesArray)
            {
                var filteredConditionDict = GetFilteredConditionDict(rule);
                if (MatchesCondition(filteredConditionDict, inputValues))
                {
                    return rule["ruleId"]?.ToObject<int>();
                }
            }
        }

        return null;
    }

    static List<int> ExecuteAll(string json, string targetName, Dictionary<string, object> inputValues)
    {
        var unmatchedRuleIds = new List<int>();
        var rulesArray = GetRulesArray(json, targetName);
        if (rulesArray != null)
        {
            foreach (var rule in rulesArray)
            {
                var filteredConditionDict = GetFilteredConditionDict(rule);
                if (!MatchesCondition(filteredConditionDict, inputValues))
                {
                    var ruleId = rule["ruleId"]?.ToObject<int>();
                    if (ruleId.HasValue)
                    {
                        unmatchedRuleIds.Add(ruleId.Value);
                    }
                }
            }
        }

        return unmatchedRuleIds;
    }

    private static JArray? GetRulesArray(string json, string targetName)
    {
        var jsonObject = JObject.Parse(json);

        if (jsonObject["conditions"] is JArray conditionsArray)
        {
            var targetCondition = conditionsArray.FirstOrDefault(c => c["name"]?.ToString() == targetName);
            if (targetCondition != null && targetCondition["rules"] is JArray rulesArray)
                return rulesArray;
        }
        return null;
    }

    static Dictionary<string, object> GetFilteredConditionDict(JToken rule)
    {
        var conditionDict = rule.ToObject<Dictionary<string, object>>() ?? new Dictionary<string, object>();
        var ignoredProps = rule["ignoredProps"]?.ToObject<List<string>>() ?? new List<string>();
        return conditionDict
            .Where(pair => !ignoredProps.Contains(pair.Key))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    static bool MatchesCondition(Dictionary<string, object> condition, Dictionary<string, object> input)
    {
        return condition.All(pair => input.TryGetValue(pair.Key, out var value) && value is not null && Equals(value.ToString(), pair.Value.ToString()));
    }
}

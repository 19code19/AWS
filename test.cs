using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class RuleEngine
{
    public string match { get; set; }
    public List<Dictionary<string, object>> rules { get; set; }
}

class Program
{
    static void Main()
    {
        string jsonFilePath = "conditions.json";
        string json = File.ReadAllText(jsonFilePath);
        IDictionary<string, RuleEngine> ruleEngines = JsonConvert.DeserializeObject<IDictionary<string, RuleEngine>>(json);
        string targetName = "Tpp";

        var inputValues = new Dictionary<string, object>
        {
            { "type", "test" },
            { "paymenttype", 2 },
            { "city", 5 }
        };

        if (!ruleEngines.TryGetValue(targetName, out var targetObject))
        {
            Console.WriteLine("Target not found.");
            return;
        }

        string matchType = targetObject.match;
        if (matchType == "Any")
        {
            int? matchingRuleId = ExecuteAny(targetObject, inputValues);
            if (matchingRuleId.HasValue)
            {
                Console.WriteLine("Match Found: Rule ID = " + matchingRuleId.Value);
            }
            else
            {
                Console.WriteLine("No Match Found");
            }
        }
        else if (matchType == "All")
        {
            var unmatchedRuleIds = ExecuteAll(targetObject, inputValues);
            if (unmatchedRuleIds.Any())
            {
                Console.WriteLine("Unmatched Rule IDs: " + string.Join(", ", unmatchedRuleIds));
            }
            else
            {
                Console.WriteLine("All rules matched");
            }
        }
    }

    static int? ExecuteAny(RuleEngine ruleEngine, Dictionary<string, object> inputValues)
    {
        foreach (var rule in ruleEngine.rules)
        {
            var filteredConditionDict = GetFilteredConditionDict(rule);
            if (MatchesCondition(filteredConditionDict, inputValues))
            {
                var ruleId = Convert.ToInt32(rule["ruleId"]);
                return ruleId <= 0 ? null : ruleId;
            }
        }
        return null;
    }

    static List<int> ExecuteAll(RuleEngine ruleEngine, Dictionary<string, object> inputValues)
    {
        var unmatchedRuleIds = new List<int>();
        foreach (var rule in ruleEngine.rules)
        {
            var filteredConditionDict = GetFilteredConditionDict(rule);
            if (!MatchesCondition(filteredConditionDict, inputValues))
            {
                var ruleId = Convert.ToInt32(rule["ruleId"]);
                if (ruleId > 0)
                {
                    unmatchedRuleIds.Add(ruleId);
                }
            }
        }
        return unmatchedRuleIds;
    }

    static Dictionary<string, object> GetFilteredConditionDict(Dictionary<string, object> rule)
    {
        var ignoredProps = rule["ignoredProps"] as List<object> ?? [];
        return rule
            .Where(pair => !ignoredProps.Contains(pair.Key))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    static bool MatchesCondition(Dictionary<string, object> condition, Dictionary<string, object> input)
    {
        return condition.All(pair => input.TryGetValue(pair.Key, out var value) && value is not null && Equals(value.ToString(), pair.Value.ToString()));
    }
}


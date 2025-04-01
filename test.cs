using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using RulesEngine.Models;
using RulesEngine;

class Program
{
    static async Task Main()
    {
        // Load rules from JSON file
        string json = File.ReadAllText("rules.json");
        var workflows = JsonSerializer.Deserialize<List<Workflow>>(json);

        if (workflows == null) return;

        RulesEngine rulesEngine = new RulesEngine(workflows.ToArray());

        // Sample input data
        var inputData = new Dictionary<string, object>
        {
            { "type", "test" },
            { "paymenttype", 2 },
            { "city", 5 },
            { "state", "3" }
        };

        // Select client workflow (e.g., "Tpp" or "Abc")
        string clientName = "Tpp";  // Change this to test "Abc"
        var workflow = workflows.Find(w => w.WorkflowName == clientName);

        if (workflow == null)
        {
            Console.WriteLine($"No rules found for client: {clientName}");
            return;
        }

        // Check rules one by one and stop after first success
        foreach (var rule in workflow.Rules)
        {
            RuleResultTree[] result = await rulesEngine.ExecuteAllRulesAsync(clientName, inputData);

            foreach (var r in result)
            {
                if (r.IsSuccess)
                {
                    Console.WriteLine($"Rule Passed: {r.Rule.SuccessEvent}");
                    return; // Stop execution after first match
                }
            }
        }

        Console.WriteLine("No rules matched.");
    }
}

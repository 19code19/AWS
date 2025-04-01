using NRules;
using NRules.Fluent;
using NRules.Fluent.Dsl;
using System;
using System.Collections.Generic;
  <PackageReference Include="NRules" Version="1.0.2" />
// Domain model classes
public class Rule
{
    public string Type { get; set; }
    public int Paymenttype { get; set; }
    public int? City { get; set; }
    public string State { get; set; }
    public string Status { get; set; } // Added status to track if the rule passed or failed
}

public class RuleSet
{
    public List<Rule> Rules { get; set; }
}

public class RuleData
{
    public RuleSet Tpp { get; set; }
    public RuleSet Abc { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        // 1. Create a RuleRepository to load the rules
        var repository = new RuleRepository();

        // 2. Load the rules
        repository.Load(x => x.From(typeof(TppRules), typeof(AbcRules)));

        // 3. Compile the rules
        var factory = repository.Compile();

        // 4. Create a session
        var session = factory.CreateSession();

        // 5. Sample RuleData
        var ruleData = new RuleData
        {
            Tpp = new RuleSet
            {
                Rules = new List<Rule>
                {
                    new Rule { Type = "test", Paymenttype = 2 },
                    new Rule { Type = "test", Paymenttype = 2, City = 5 },
                    new Rule { Type = "test", Paymenttype = 2, City = 5, State = "3" }
                }
            },
            Abc = new RuleSet
            {
                Rules = new List<Rule>
                {
                    new Rule { Type = "test", Paymenttype = 1, City = 4 },
                    new Rule { Type = "test", Paymenttype = 3, State = "2" }
                }
            }
        };

        // 6. Insert the rule data into the session
        session.Insert(ruleData);

        // 7. Fire the rules
        session.Fire();

        // Print out the rule statuses
        foreach (var rule in ruleData.Tpp.Rules)
        {
            Console.WriteLine($"Status: {rule.Status}");
        }
        foreach (var rule in ruleData.Abc.Rules)
        {
            Console.WriteLine($"Status: {rule.Status}");
        }
    }
}

// Rule definitions
public class TppRules : NRules.Fluent.Dsl.Rule
{
    public override void Define()
    {
        RuleData ruleData = null;

        When()
            .Match<RuleData>(() => ruleData, rd => rd.Tpp != null);

        Then()
            .Do(ctx => ProcessTppRules(ruleData));
    }

    private void ProcessTppRules(RuleData ruleData)
    {
        foreach (var rule in ruleData.Tpp.Rules)
        {
            // Evaluate the rule conditions
            if (rule.Paymenttype == 2)
            {
                // Rule passed, update status to "Passed"
                rule.Status = "Passed";
                Console.WriteLine($"Paymenttype = {rule.Paymenttype}, City = {rule.City}, State = {rule.State}, Status = Passed");
            }
            else
            {
                // Rule failed, update status to "Failed"
                rule.Status = "Failed";
                Console.WriteLine($"Paymenttype = {rule.Paymenttype}, City = {rule.City}, State = {rule.State}, Status = Failed");
            }
        }
    }
}

public class AbcRules : NRules.Fluent.Dsl.Rule
{
    public override void Define()
    {
        RuleData ruleData = null;

        When()
            .Match<RuleData>(() => ruleData, rd => rd.Abc != null);

        Then()
            .Do(ctx => ProcessAbcRules(ruleData));
    }

    private static void ProcessAbcRules(RuleData ruleData)
    {
        foreach (var rule in ruleData.Abc.Rules)
        {
            // Evaluate the rule conditions
            if (rule.Paymenttype == 1)
            {
                // Rule passed, update status to "Passed"
                rule.Status = "Passed";
                Console.WriteLine($"Paymenttype = {rule.Paymenttype}, City = {rule.City}, State = {rule.State}, Status = Passed");
            }
            else
            {
                // Rule failed, update status to "Failed"
                rule.Status = "Failed";
                Console.WriteLine($"Paymenttype = {rule.Paymenttype}, City = {rule.City}, State = {rule.State}, Status = Failed");
            }
        }
    }
}

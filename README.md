await_context.Employees
    .Where(emp => bankerSids.Any(sid => sid.Equals(emp.Sid.ToLower())) && 
                  (emp.IsActive == false || emp.IsActive == true))
    .ExecuteUpdateAsync(setters => setters
        .SetProperty(b => b.IsActive, emp => bankerSids.Any(sid => sid.Equals(emp.Sid.ToLower())))
        .SetProperty(b => b.Modified, date));

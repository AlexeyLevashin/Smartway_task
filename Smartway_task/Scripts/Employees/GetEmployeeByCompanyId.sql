SELECT e.id, e.name as "Name", e.surname as "Surname", e.phone as "Phone", e.companyid as "CompanyId", e.departmentid as "DepartmentId",
        p.id, p.type as "Type", p.number as "Number", p.employeeid as "EmployeeId",
        d.id, d.name as "Name", d.phone as "Phone"
FROM employees e
         LEFT JOIN passports p ON p.EmployeeId = e.Id
         LEFT JOIN departments d ON d.Id = e.DepartmentId
WHERE e.companyid = @companyId
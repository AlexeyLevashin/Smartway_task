SELECT id, name as "Name", surname as "Surname", phone as "Phone", companyid as "CompanyId", departmentid as "DepartmentId"
FROM employees
WHERE id = @id
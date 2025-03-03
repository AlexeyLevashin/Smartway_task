UPDATE employees SET name = @name, surname = @surname, phone = @phone, companyid = @companyid, departmentid = @departmentid
WHERE id = @Id
    RETURNING id as "Id", name as "Name", surname as "Surname", phone as "Phone", companyid as "CompanyId", departmentid as "DepartmentId"
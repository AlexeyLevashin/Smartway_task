using Smartway_task.Domain;
using Smartway_task.DTO;
using Smartway_task.Models;

namespace Smartway_task.Mappers;

public static class PassportMapper
{
    public static Passport MapToDomain(this DbPassport dbPassport)
    {
        return new Passport
        {
            Id = dbPassport.Id,
            Type = dbPassport.Type,
            Number = dbPassport.Number,
            EmployeeId = dbPassport.EmployeeId
        };
    }
    
    public static PassportResponseDto MapToDto(this Passport passport)
    {
        return new PassportResponseDto
        {
            Id = passport.Id,
            Type = passport.Type,
            Number = passport.Number
        };
    }
    
    public static DbPassport MapToDb(this Passport passport)
    {
        return new DbPassport
        {
            Id = passport.Id,
            Type = passport.Type,
            Number = passport.Number
        };
    }
}
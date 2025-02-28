using Smartway_task.Domain;
using Smartway_task.DTO;
using Smartway_task.Models;

namespace Smartway_task.Mappers;

public static class PassportMapper
{
    public static Passport MapToDomain(this AddNewPassportRequestDto addNewPassportRequestDto)
    {
        return new Passport
        {
            Type = addNewPassportRequestDto.Type,
            Number = addNewPassportRequestDto.Number,
        };
    }
    
    
    public static Passport MapToDomain(this DbPassport dbpassport)
    {
        return new Passport
        {
            Id = dbpassport.Id,
            Type = dbpassport.Type,
            Number = dbpassport.Number,
            EmployeeId = dbpassport.EmployeeId
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
            Number = passport.Number,
        };
    }
}
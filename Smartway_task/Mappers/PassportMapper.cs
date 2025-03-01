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
    
    public static Passport MapToDomain(this PassportResponseDto passportResponseDto)
    {
        return new Passport
        {
            Id = passportResponseDto.Id,
            Type = passportResponseDto.Type,
            Number = passportResponseDto.Number,
            EmployeeId = passportResponseDto.EmployeeId
        };
    }
    
    public static PassportResponseDto MapToDto(this Passport passport)
    {
        return new PassportResponseDto
        {
            Id = passport.Id,
            Type = passport.Type,
            Number = passport.Number,
            EmployeeId = passport.EmployeeId
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
    
    public static DbPassport ApplyChangesFromDto(this DbPassport existingPassport, UpdatePassportRequestDto passportDto)
    {
        existingPassport.Type = passportDto.Type ?? existingPassport.Type;
        existingPassport.Number = passportDto.Number ?? existingPassport.Number;
        return existingPassport;
    }


}
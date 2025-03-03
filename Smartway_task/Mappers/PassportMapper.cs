using Smartway_task.Domain;
using Smartway_task.Models;
using Smartway_task.DTO.Passport.Requests;
using Smartway_task.DTO.Passport.Responses;

namespace Smartway_task.Mappers;

public static class PassportMapper
{
    public static DbPassport MapToDb(this NewPassportRequest dto)
    {
        return new DbPassport
        {
            Type = dto.Type,
            Number = dto.Number
        };
    }

    public static PassportResponse MapToDto(this DbPassport dbPassport)
    {
        return new PassportResponse
        {
            Id = dbPassport.Id,
            Type = dbPassport.Type,
            Number = dbPassport.Number
        };
    }

    public static PassportResponse MapToDto(this Passport passport)
    {
        return new PassportResponse
        {
            Id = passport.Id,
            Type = passport.Type,
            Number = passport.Number,
            EmployeeId = passport.EmployeeId
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

    public static DbPassport ApplyChangesFromDto(this DbPassport existingPassport, PassportUpdateRequest passportDto)
    {
        existingPassport.Type = passportDto.Type ?? existingPassport.Type;
        existingPassport.Number = passportDto.Number ?? existingPassport.Number;

        return existingPassport;
    }
}
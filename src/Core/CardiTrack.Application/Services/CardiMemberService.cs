using CardiTrack.Application.DTOs.Requests;
using CardiTrack.Application.DTOs.Responses;
using CardiTrack.Application.Interfaces.Services;
using CardiTrack.Domain.Entities;
using CardiTrack.Application.Interfaces.Repositories;

namespace CardiTrack.Application.Services;

public class CardiMemberService : ICardiMemberService
{
    private readonly IUnitOfWork _unitOfWork;

    public CardiMemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CardiMemberResponse> CreateCardiMemberAsync(
        Guid organizationId,
        Guid userId,
        CreateCardiMemberRequest request)
    {
        var cardiMember = new CardiMember
        {
            OrganizationId = organizationId,
            Name = request.Name,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            Email = request.Email,
            Phone = request.Phone,
            EmergencyContactName = request.EmergencyContactName,
            EmergencyContactPhone = request.EmergencyContactPhone,
            MedicalNotes = request.MedicalNotes, // TODO: Encrypt this
            IsActive = true
        };

        await _unitOfWork.CardiMembers.AddAsync(cardiMember);
        await _unitOfWork.SaveChangesAsync(); // Save to get ID

        // Create relationship between user and CardiMember
        var userCardiMember = new UserCardiMember
        {
            UserId = userId,
            CardiMemberId = cardiMember.Id,
            RelationshipType = request.RelationshipType,
            IsPrimaryCaregiver = request.IsPrimaryCaregiver,
            CanViewHealthData = true,
            ReceiveAlerts = true
        };

        await _unitOfWork.UserCardiMembers.AddAsync(userCardiMember);
        await _unitOfWork.SaveChangesAsync();

        return new CardiMemberResponse
        {
            Id = cardiMember.Id,
            Name = cardiMember.Name,
            DateOfBirth = cardiMember.DateOfBirth,
            Age = CalculateAge(cardiMember.DateOfBirth),
            Gender = cardiMember.Gender,
            Email = cardiMember.Email,
            Phone = cardiMember.Phone,
            Relationship = request.RelationshipType,
            IsPrimaryCaregiver = request.IsPrimaryCaregiver,
            IsActive = cardiMember.IsActive,
            CreatedDate = cardiMember.CreatedDate
        };
    }

    public async Task<CardiMemberResponse?> GetByIdAsync(Guid id)
    {
        var cardiMember = await _unitOfWork.CardiMembers.GetByIdAsync(id);
        if (cardiMember == null) return null;

        // Get relationship info - assuming first relationship for now
        var relationships = await _unitOfWork.UserCardiMembers.GetByCardiMemberIdAsync(id);
        var primaryRelationship = relationships.FirstOrDefault();

        return new CardiMemberResponse
        {
            Id = cardiMember.Id,
            Name = cardiMember.Name,
            DateOfBirth = cardiMember.DateOfBirth,
            Age = CalculateAge(cardiMember.DateOfBirth),
            Gender = cardiMember.Gender,
            Email = cardiMember.Email,
            Phone = cardiMember.Phone,
            Relationship = primaryRelationship?.RelationshipType ?? Domain.Enums.RelationshipType.Other,
            IsPrimaryCaregiver = primaryRelationship?.IsPrimaryCaregiver ?? false,
            IsActive = cardiMember.IsActive,
            CreatedDate = cardiMember.CreatedDate
        };
    }

    public async Task<List<CardiMemberResponse>> GetByOrganizationIdAsync(Guid organizationId)
    {
        var cardiMembers = await _unitOfWork.CardiMembers.GetByOrganizationIdAsync(organizationId);
        var responses = new List<CardiMemberResponse>();

        foreach (var cm in cardiMembers)
        {
            var relationships = await _unitOfWork.UserCardiMembers.GetByCardiMemberIdAsync(cm.Id);
            var primaryRelationship = relationships.FirstOrDefault();

            responses.Add(new CardiMemberResponse
            {
                Id = cm.Id,
                Name = cm.Name,
                DateOfBirth = cm.DateOfBirth,
                Age = CalculateAge(cm.DateOfBirth),
                Gender = cm.Gender,
                Email = cm.Email,
                Phone = cm.Phone,
                Relationship = primaryRelationship?.RelationshipType ?? Domain.Enums.RelationshipType.Other,
                IsPrimaryCaregiver = primaryRelationship?.IsPrimaryCaregiver ?? false,
                IsActive = cm.IsActive,
                CreatedDate = cm.CreatedDate
            });
        }

        return responses;
    }

    private int CalculateAge(DateOnly dateOfBirth)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - dateOfBirth.Year;
        if (today.Month < dateOfBirth.Month || (today.Month == dateOfBirth.Month && today.Day < dateOfBirth.Day))
        {
            age--;
        }
        return age;
    }
}

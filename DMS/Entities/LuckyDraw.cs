using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.Entities
{
    public class LuckyDraw : DataEntity, IEquatable<LuckyDraw>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long LuckyDrawTypeId { get; set; }
        public long OrganizationId { get; set; }
        public decimal RevenuePerTurn { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public long AvatarImageId { get; set; }
        public long ImageId { get; set; }
        public long StoreId { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public long LuckyDrawNumberId { get; set; }
        public bool Used { get; set; }
        public Image AvatarImage { get; set; }
        public Image Image { get; set; }
        public LuckyDrawType LuckyDrawType { get; set; }
        public Organization Organization { get; set; }
        public Status Status { get; set; }
        public List<LuckyDrawStoreGroupingMapping> LuckyDrawStoreGroupingMappings { get; set; }
        public List<LuckyDrawStoreMapping> LuckyDrawStoreMappings { get; set; }
        public List<LuckyDrawStoreTypeMapping> LuckyDrawStoreTypeMappings { get; set; }
        public List<LuckyDrawStructure> LuckyDrawStructures { get; set; }
        public LuckyDrawNumber LuckyDrawNumber { get; set; }
        public List<LuckyDrawWinner> LuckyDrawWinners { get; set; }
        public List<LuckyDrawRegistration> LuckyDrawRegistrations { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool Equals(LuckyDraw other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.Code != other.Code) return false;
            if (this.Name != other.Name) return false;
            if (this.LuckyDrawTypeId != other.LuckyDrawTypeId) return false;
            if (this.OrganizationId != other.OrganizationId) return false;
            if (this.RevenuePerTurn != other.RevenuePerTurn) return false;
            if (this.StartAt != other.StartAt) return false;
            if (this.EndAt != other.EndAt) return false;
            if (this.AvatarImageId != other.AvatarImageId) return false;
            if (this.ImageId != other.ImageId) return false;
            if (this.Description != other.Description) return false;
            if (this.StatusId != other.StatusId) return false;
            if (this.Used != other.Used) return false;
            if (this.LuckyDrawStoreGroupingMappings?.Count != other.LuckyDrawStoreGroupingMappings?.Count) return false;
            else if (this.LuckyDrawStoreGroupingMappings != null && other.LuckyDrawStoreGroupingMappings != null)
            {
                for (int i = 0; i < LuckyDrawStoreGroupingMappings.Count; i++)
                {
                    LuckyDrawStoreGroupingMapping LuckyDrawStoreGroupingMapping = LuckyDrawStoreGroupingMappings[i];
                    LuckyDrawStoreGroupingMapping otherLuckyDrawStoreGroupingMapping = other.LuckyDrawStoreGroupingMappings[i];
                    if (LuckyDrawStoreGroupingMapping == null && otherLuckyDrawStoreGroupingMapping != null)
                        return false;
                    if (LuckyDrawStoreGroupingMapping != null && otherLuckyDrawStoreGroupingMapping == null)
                        return false;
                    if (LuckyDrawStoreGroupingMapping.Equals(otherLuckyDrawStoreGroupingMapping) == false)
                        return false;
                }
            }
            if (this.LuckyDrawStoreMappings?.Count != other.LuckyDrawStoreMappings?.Count) return false;
            else if (this.LuckyDrawStoreMappings != null && other.LuckyDrawStoreMappings != null)
            {
                for (int i = 0; i < LuckyDrawStoreMappings.Count; i++)
                {
                    LuckyDrawStoreMapping LuckyDrawStoreMapping = LuckyDrawStoreMappings[i];
                    LuckyDrawStoreMapping otherLuckyDrawStoreMapping = other.LuckyDrawStoreMappings[i];
                    if (LuckyDrawStoreMapping == null && otherLuckyDrawStoreMapping != null)
                        return false;
                    if (LuckyDrawStoreMapping != null && otherLuckyDrawStoreMapping == null)
                        return false;
                    if (LuckyDrawStoreMapping.Equals(otherLuckyDrawStoreMapping) == false)
                        return false;
                }
            }
            if (this.LuckyDrawStoreTypeMappings?.Count != other.LuckyDrawStoreTypeMappings?.Count) return false;
            else if (this.LuckyDrawStoreTypeMappings != null && other.LuckyDrawStoreTypeMappings != null)
            {
                for (int i = 0; i < LuckyDrawStoreTypeMappings.Count; i++)
                {
                    LuckyDrawStoreTypeMapping LuckyDrawStoreTypeMapping = LuckyDrawStoreTypeMappings[i];
                    LuckyDrawStoreTypeMapping otherLuckyDrawStoreTypeMapping = other.LuckyDrawStoreTypeMappings[i];
                    if (LuckyDrawStoreTypeMapping == null && otherLuckyDrawStoreTypeMapping != null)
                        return false;
                    if (LuckyDrawStoreTypeMapping != null && otherLuckyDrawStoreTypeMapping == null)
                        return false;
                    if (LuckyDrawStoreTypeMapping.Equals(otherLuckyDrawStoreTypeMapping) == false)
                        return false;
                }
            }
            if (this.LuckyDrawStructures?.Count != other.LuckyDrawStructures?.Count) return false;
            else if (this.LuckyDrawStructures != null && other.LuckyDrawStructures != null)
            {
                for (int i = 0; i < LuckyDrawStructures.Count; i++)
                {
                    LuckyDrawStructure LuckyDrawStructure = LuckyDrawStructures[i];
                    LuckyDrawStructure otherLuckyDrawStructure = other.LuckyDrawStructures[i];
                    if (LuckyDrawStructure == null && otherLuckyDrawStructure != null)
                        return false;
                    if (LuckyDrawStructure != null && otherLuckyDrawStructure == null)
                        return false;
                    if (LuckyDrawStructure.Equals(otherLuckyDrawStructure) == false)
                        return false;
                }
            }
            if (this.LuckyDrawWinners?.Count != other.LuckyDrawWinners?.Count) return false;
            else if (this.LuckyDrawWinners != null && other.LuckyDrawWinners != null)
            {
                for (int i = 0; i < LuckyDrawWinners.Count; i++)
                {
                    LuckyDrawWinner LuckyDrawWinner = LuckyDrawWinners[i];
                    LuckyDrawWinner otherLuckyDrawWinner = other.LuckyDrawWinners[i];
                    if (LuckyDrawWinner == null && otherLuckyDrawWinner != null)
                        return false;
                    if (LuckyDrawWinner != null && otherLuckyDrawWinner == null)
                        return false;
                    if (LuckyDrawWinner.Equals(otherLuckyDrawWinner) == false)
                        return false;
                }
            }
            if (this.LuckyDrawRegistrations?.Count != other.LuckyDrawRegistrations?.Count) return false;
            else if (this.LuckyDrawRegistrations != null && other.LuckyDrawRegistrations != null)
            {
                for (int i = 0; i < LuckyDrawRegistrations.Count; i++)
                {
                    LuckyDrawRegistration LuckyDrawRegistration = LuckyDrawRegistrations[i];
                    LuckyDrawRegistration otherLuckyDrawRegistration = other.LuckyDrawRegistrations[i];
                    if (LuckyDrawRegistration == null && LuckyDrawRegistration != null)
                        return false;
                    if (LuckyDrawRegistration != null && otherLuckyDrawRegistration == null)
                        return false;
                    if (LuckyDrawRegistration.Equals(otherLuckyDrawRegistration) == false)
                        return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class LuckyDrawFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter LuckyDrawTypeId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public DecimalFilter RevenuePerTurn { get; set; }
        public DateFilter StartAt { get; set; }
        public DateFilter EndAt { get; set; }
        public IdFilter AvatarImageId { get; set; }
        public IdFilter ImageId { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter StatusId { get; set; }
        public bool? Used { get; set; }
        public bool? IsInAction { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<LuckyDrawFilter> OrFilter { get; set; }
        public LuckyDrawOrder OrderBy { get; set; }
        public LuckyDrawSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LuckyDrawOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        LuckyDrawType = 3,
        Organization = 4,
        RevenuePerTurn = 5,
        StartAt = 6,
        EndAt = 7,
        AvatarImage = 8,
        Image = 9,
        Description = 10,
        Status = 11,
        Used = 12,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum LuckyDrawSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        LuckyDrawType = E._3,
        Organization = E._4,
        RevenuePerTurn = E._5,
        StartAt = E._6,
        EndAt = E._7,
        AvatarImage = E._8,
        Image = E._9,
        Description = E._10,
        Status = E._11,
        Used = E._12,
        LuckyDrawStructures = E._13,
    }
}

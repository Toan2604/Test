using DMS.Entities;
using DMS.Enums;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface ISurveyResultRepository
    {
        Task<int> Count(SurveyResultFilter SurveyResultFilter);
        Task<List<SurveyResult>> List(SurveyResultFilter SurveyResultFilter);
        Task<SurveyResult> Get(long Id);
        Task<bool> Create(SurveyResult SurveyResult);
        Task<bool> Delete(SurveyResult SurveyResult);
    }
    public class SurveyResultRepository : ISurveyResultRepository
    {
        private DataContext DataContext;
        public SurveyResultRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<SurveyResultDAO> DynamicFilter(IQueryable<SurveyResultDAO> query, SurveyResultFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.AppUserId, filter.AppUserId);
            query = query.Where(q => q.SurveyId, filter.SurveyId);
            query = query.Where(q => q.StoreId, filter.StoreId);
            if (filter.OrganizationId != null)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value).FirstOrDefault();
                    query = query.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefault();
                    query = query.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
            }
            query = query.Where(q => q.Time, filter.Time);
            return query;
        }

        private IQueryable<SurveyResultDAO> DynamicOrder(IQueryable<SurveyResultDAO> query, SurveyResultFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case SurveyResultOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case SurveyResultOrder.AppUser:
                            query = query.OrderBy(q => q.AppUser.DisplayName);
                            break;
                        case SurveyResultOrder.Store:
                            query = query.OrderBy(q => q.Store.Name);
                            break;
                        case SurveyResultOrder.Survey:
                            query = query.OrderBy(q => q.Survey.Title);
                            break;
                        case SurveyResultOrder.Time:
                            query = query.OrderBy(q => q.Time);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case SurveyResultOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case SurveyResultOrder.AppUser:
                            query = query.OrderByDescending(q => q.AppUser.DisplayName);
                            break;
                        case SurveyResultOrder.Store:
                            query = query.OrderByDescending(q => q.Store.Name);
                            break;
                        case SurveyResultOrder.Survey:
                            query = query.OrderByDescending(q => q.Survey.Title);
                            break;
                        case SurveyResultOrder.Time:
                            query = query.OrderByDescending(q => q.Time);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<SurveyResult>> DynamicSelect(IQueryable<SurveyResultDAO> query, SurveyResultFilter filter)
        {
            List<SurveyResult> SurveyResults = await query.Select(q => new SurveyResult()
            {
                Id = filter.Selects.Contains(SurveyResultSelect.Id) ? q.Id : default(long),
                AppUserId = filter.Selects.Contains(SurveyResultSelect.AppUser) ? q.AppUserId : default(long),
                StoreId = filter.Selects.Contains(SurveyResultSelect.Store) ? q.StoreId : default(long),
                StoreScoutingId = filter.Selects.Contains(SurveyResultSelect.StoreScouting) ? q.StoreScoutingId : default(long),
                SurveyId = filter.Selects.Contains(SurveyResultSelect.Survey) ? q.SurveyId : default(long),
                Time = filter.Selects.Contains(SurveyResultSelect.Time) ? q.Time : default(DateTime),
                RespondentAddress = filter.Selects.Contains(SurveyResultSelect.RespondentAddress) ? q.RespondentAddress : default(string),
                RespondentEmail = filter.Selects.Contains(SurveyResultSelect.RespondentEmail) ? q.RespondentEmail : default(string),
                RespondentName = filter.Selects.Contains(SurveyResultSelect.RespondentName) ? q.RespondentName : default(string),
                RespondentPhone = filter.Selects.Contains(SurveyResultSelect.RespondentPhone) ? q.RespondentPhone : default(string),
                SurveyRespondentTypeId = filter.Selects.Contains(SurveyResultSelect.SurveyRespondentType) ? q.SurveyRespondentTypeId : default(long),
                AppUser = filter.Selects.Contains(SurveyResultSelect.AppUser) && q.AppUser != null ? new AppUser
                {
                    Id = q.AppUser.Id,
                    Username = q.AppUser.Username,
                    DisplayName = q.AppUser.DisplayName,
                    Address = q.AppUser.Address,
                    Email = q.AppUser.Email,
                    Phone = q.AppUser.Phone,
                    Organization = q.AppUser.Organization == null ? null : new Organization
                    {
                        Id = q.AppUser.Organization.Id,
                        Code = q.AppUser.Organization.Code,
                        Name = q.AppUser.Organization.Name,
                    },
                } : null,
                Store = filter.Selects.Contains(SurveyResultSelect.Store) && q.Store != null ? new Store
                {
                    Id = q.Store.Id,
                    Code = q.Store.Code,
                    Name = q.Store.Name,
                    Address = q.Store.Address,
                    Telephone = q.Store.Telephone,
                    Organization = q.Store.Organization == null ? null : new Organization
                    {
                        Id = q.Store.Organization.Id,
                        Code = q.Store.Organization.Code,
                        Name = q.Store.Organization.Name,
                    },
                } : null,
                StoreScouting = filter.Selects.Contains(SurveyResultSelect.StoreScouting) && q.StoreScouting != null ? new StoreScouting
                {
                    Id = q.StoreScouting.Id,
                    Code = q.StoreScouting.Code,
                    Name = q.StoreScouting.Name,
                    Address = q.StoreScouting.Address,
                    OwnerPhone = q.StoreScouting.OwnerPhone,
                    Organization = q.StoreScouting.Organization == null ? null : new Organization
                    {
                        Id = q.StoreScouting.Organization.Id,
                        Code = q.StoreScouting.Organization.Code,
                        Name = q.StoreScouting.Organization.Name,
                    },
                } : null,
                SurveyRespondentType = filter.Selects.Contains(SurveyResultSelect.SurveyRespondentType) && q.SurveyRespondentType == null ? null : new SurveyRespondentType
                {
                    Id = q.SurveyRespondentType.Id,
                    Code = q.SurveyRespondentType.Code,
                    Name = q.SurveyRespondentType.Name,
                },
            }).ToListWithNoLockAsync();

            if (filter.Selects.Contains(SurveyResultSelect.Content))
            {
                var SurveyResultIds = SurveyResults.Select(x => x.Id).ToList();
                IdFilter SurveyResultIdFilter = new IdFilter { In = SurveyResultIds };
                var SurveyResultTexts = await DataContext.SurveyResultText
                    .Where(x => x.SurveyResultId, SurveyResultIdFilter)
                    .ToListWithNoLockAsync();
                var SurveyResultSingles = await DataContext.SurveyResultSingle
                    .Where(x => x.SurveyResultId, SurveyResultIdFilter)
                    .ToListWithNoLockAsync();
                var SurveyResultCells = await DataContext.SurveyResultCell
                    .Where(x => x.SurveyResultId, SurveyResultIdFilter)
                    .ToListWithNoLockAsync();
                foreach (var SurveyResult in SurveyResults)
                {
                    SurveyResult.SurveyResultTexts = SurveyResultTexts.Where(x => x.SurveyResultId == SurveyResult.Id).Select(x => new SurveyResultText
                    {
                        SurveyQuestionId = x.SurveyQuestionId,
                        SurveyResultId = x.SurveyResultId,
                        Content = x.Content,
                    }).ToList();
                    SurveyResult.SurveyResultSingles = SurveyResultSingles.Where(x => x.SurveyResultId == SurveyResult.Id).Select(x => new SurveyResultSingle
                    {
                        SurveyOptionId = x.SurveyOptionId,
                        SurveyQuestionId = x.SurveyQuestionId,
                        SurveyResultId = x.SurveyResultId,
                    }).ToList();
                    SurveyResult.SurveyResultCells = SurveyResultCells.Where(x => x.SurveyResultId == SurveyResult.Id).Select(x => new SurveyResultCell
                    {
                        ColumnOptionId = x.ColumnOptionId,
                        RowOptionId = x.RowOptionId,
                        SurveyQuestionId = x.SurveyQuestionId,
                        SurveyResultId = x.SurveyResultId,
                    }).ToList();
                }
            }
            return SurveyResults;
        }

        public async Task<int> Count(SurveyResultFilter filter)
        {
            IQueryable<SurveyResultDAO> SurveyResults = DataContext.SurveyResult.AsNoTracking();
            SurveyResults = DynamicFilter(SurveyResults, filter);
            return await SurveyResults.CountWithNoLockAsync();
        }

        public async Task<List<SurveyResult>> List(SurveyResultFilter filter)
        {
            if (filter == null) return new List<SurveyResult>();
            IQueryable<SurveyResultDAO> SurveyResultDAOs = DataContext.SurveyResult.AsNoTracking();
            SurveyResultDAOs = DynamicFilter(SurveyResultDAOs, filter);
            SurveyResultDAOs = DynamicOrder(SurveyResultDAOs, filter);
            List<SurveyResult> SurveyResults = await DynamicSelect(SurveyResultDAOs, filter);
            return SurveyResults;
        }

        public async Task<SurveyResult> Get(long Id)
        {
            SurveyResult SurveyResult = await DataContext.SurveyResult.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new SurveyResult()
            {
                Id = x.Id,
                AppUserId = x.AppUserId,
                OrganizationId = x.OrganizationId,
                StoreId = x.StoreId,
                SurveyId = x.SurveyId,
                Time = x.Time,
                StoreScoutingId = x.StoreScoutingId,
                RespondentAddress = x.RespondentAddress,
                RespondentEmail = x.RespondentEmail,
                RespondentName = x.RespondentName,
                RespondentPhone = x.RespondentPhone,
                SurveyRespondentTypeId = x.SurveyRespondentTypeId,
                AppUser = x.AppUser == null ? null : new AppUser
                {
                    Id = x.AppUser.Id,
                    Username = x.AppUser.Username,
                    DisplayName = x.AppUser.DisplayName,
                },
                Store = x.Store == null ? null : new Store
                {
                    Id = x.Store.Id,
                    Code = x.Store.Code,
                    Name = x.Store.Name,
                },
                StoreScouting = x.StoreScouting == null ? null : new StoreScouting
                {
                    Id = x.StoreScouting.Id,
                    Code = x.StoreScouting.Code,
                    Name = x.StoreScouting.Name,
                },
                SurveyRespondentType = x.SurveyRespondentType == null ? null : new SurveyRespondentType
                {
                    Id = x.SurveyRespondentType.Id,
                    Code = x.SurveyRespondentType.Code,
                    Name = x.SurveyRespondentType.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (SurveyResult == null)
                return null;
            SurveyResult.SurveyResultCells = await DataContext.SurveyResultCell.AsNoTracking()
                .Where(x => x.SurveyResultId == SurveyResult.Id)
                .Select(x => new SurveyResultCell
                {
                    SurveyResultId = x.SurveyResultId,
                    SurveyQuestionId = x.SurveyQuestionId,
                    ColumnOptionId = x.ColumnOptionId,
                    RowOptionId = x.RowOptionId,
                }).ToListWithNoLockAsync();
            SurveyResult.SurveyResultSingles = await DataContext.SurveyResultSingle.AsNoTracking()
                .Where(x => x.SurveyResultId == SurveyResult.Id)
                .Select(x => new SurveyResultSingle
                {
                    SurveyResultId = x.SurveyResultId,
                    SurveyQuestionId = x.SurveyQuestionId,
                    SurveyOptionId = x.SurveyOptionId,
                }).ToListWithNoLockAsync();
            SurveyResult.SurveyResultTexts = await DataContext.SurveyResultText.AsNoTracking()
                .Where(x => x.SurveyResultId == SurveyResult.Id)
                .Select(x => new SurveyResultText
                {
                    SurveyResultId = x.SurveyResultId,
                    SurveyQuestionId = x.SurveyQuestionId,
                    Content = x.Content,
                }).ToListWithNoLockAsync();
            return SurveyResult;
        }

        public async Task<bool> Create(SurveyResult SurveyResult)
        {
            SurveyDAO SurveyDAO = await DataContext.Survey.Where(s => s.Id == SurveyResult.SurveyId).FirstOrDefaultWithNoLockAsync();
            SurveyDAO.Used = true;
            SurveyResult.RowId = Guid.NewGuid();
            SurveyResultDAO SurveyResultDAO = new SurveyResultDAO();
            SurveyResultDAO.AppUserId = SurveyResult.AppUserId;
            SurveyResultDAO.OrganizationId = SurveyResult.OrganizationId;
            SurveyResultDAO.RespondentPhone = SurveyResult.RespondentPhone;
            SurveyResultDAO.RespondentName = SurveyResult.RespondentName;
            SurveyResultDAO.RespondentEmail = SurveyResult.RespondentEmail;
            SurveyResultDAO.RespondentAddress = SurveyResult.RespondentAddress;
            SurveyResultDAO.StoreId = SurveyResult.StoreId;
            SurveyResultDAO.StoreScoutingId = SurveyResult.StoreScoutingId;
            SurveyResultDAO.SurveyId = SurveyResult.SurveyId;
            SurveyResultDAO.Time = SurveyResult.Time;
            SurveyResultDAO.RowId = SurveyResult.RowId;
            if (SurveyResultDAO.StoreId.HasValue)
                SurveyResultDAO.SurveyRespondentTypeId = SurveyRespondentTypeEnum.STORE.Id;
            else if (SurveyResultDAO.StoreScoutingId.HasValue)
                SurveyResultDAO.SurveyRespondentTypeId = SurveyRespondentTypeEnum.STORE_SCOUTING.Id;
            else
                SurveyResultDAO.SurveyRespondentTypeId = SurveyRespondentTypeEnum.OTHER.Id;

            DataContext.SurveyResult.Add(SurveyResultDAO);
            await DataContext.SaveChangesAsync();
            SurveyResult.Id = SurveyResultDAO.Id;
            await SaveReference(SurveyResult);
            return true;
        }


        public async Task<bool> Delete(SurveyResult SurveyResult)
        {
            await DataContext.SurveyResult.Where(x => x.Id == SurveyResult.Id).DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(SurveyResult SurveyResult)
        {
            List<SurveyResultSingleDAO> SurveyResultSingleDAOs = SurveyResult.SurveyResultSingles
                .Select(s => new
                {
                    SurveyResultId = SurveyResult.Id,
                    s.SurveyOptionId,
                    s.SurveyQuestionId,
                }).Distinct()
                .Select(s => new SurveyResultSingleDAO
                {
                    SurveyResultId = s.SurveyResultId,
                    SurveyOptionId = s.SurveyOptionId,
                    SurveyQuestionId = s.SurveyQuestionId,
                }).ToList();
            List<SurveyResultCellDAO> SurveyResultCellDAOs = SurveyResult.SurveyResultCells
                 .Select(s => new
                 {
                     SurveyResultId = SurveyResult.Id,
                     s.SurveyQuestionId,
                     s.ColumnOptionId,
                     s.RowOptionId,
                 }).Distinct()
                .Select(s => new SurveyResultCellDAO
                {
                    SurveyResultId = s.SurveyResultId,
                    SurveyQuestionId = s.SurveyQuestionId,
                    ColumnOptionId = s.ColumnOptionId,
                    RowOptionId = s.RowOptionId,
                }).ToList();

            List<SurveyResultTextDAO> SurveyResultTextDAOs = SurveyResult.SurveyResultTexts
               .Select(s => new
               {
                   SurveyResultId = SurveyResult.Id,
                   s.Content,
                   s.SurveyQuestionId,
               }).Distinct()
               .Select(s => new SurveyResultTextDAO
               {
                   SurveyResultId = s.SurveyResultId,
                   Content = s.Content,
                   SurveyQuestionId = s.SurveyQuestionId,
               }).ToList();

            await DataContext.SurveyResultText.BulkInsertAsync(SurveyResultTextDAOs);
            await DataContext.SurveyResultSingle.BulkInsertAsync(SurveyResultSingleDAOs);
            await DataContext.SurveyResultCell.BulkInsertAsync(SurveyResultCellDAOs);
        }
    }
}

using DMS.Entities;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface ICodeGeneratorRuleRepository
    {
        Task<int> Count(CodeGeneratorRuleFilter CodeGeneratorRuleFilter);
        Task<int> CountAll(CodeGeneratorRuleFilter CodeGeneratorRuleFilter);
        Task<List<CodeGeneratorRule>> List(CodeGeneratorRuleFilter CodeGeneratorRuleFilter);
        Task<List<CodeGeneratorRule>> List(List<long> Ids);
        Task<CodeGeneratorRule> Get(long Id);
        Task<bool> BulkMerge(List<CodeGeneratorRule> CodeGeneratorRules);
    }
    public class CodeGeneratorRuleRepository : ICodeGeneratorRuleRepository
    {
        private DataContext DataContext;
        public CodeGeneratorRuleRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<CodeGeneratorRuleDAO> DynamicFilter(IQueryable<CodeGeneratorRuleDAO> query, CodeGeneratorRuleFilter filter)
        {
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.EntityTypeId, filter.EntityTypeId);
            query = query.Where(q => q.AutoNumberLenth, filter.AutoNumberLenth);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.RowId, filter.RowId);
            if (filter.EntityComponentId != null && filter.EntityComponentId.HasValue)
            {
                if (filter.EntityComponentId.Equal != null)
                {
                    query = from q in query
                            join ce in DataContext.CodeGeneratorRuleEntityComponentMapping on q.Id equals ce.CodeGeneratorRuleId
                            join en in DataContext.EntityComponent on ce.EntityComponentId equals en.Id
                            where en.Id.Equals(filter.EntityComponentId.Equal)
                            select q;
                }
                if (filter.EntityComponentId.NotEqual != null)
                {
                    query = from q in query
                            join ce in DataContext.CodeGeneratorRuleEntityComponentMapping on q.Id equals ce.CodeGeneratorRuleId
                            join en in DataContext.EntityComponent on ce.EntityComponentId equals en.Id
                            where en.Id != filter.EntityComponentId.Equal
                            select q;
                }
            }
            return query;
        }

        private IQueryable<CodeGeneratorRuleDAO> OrFilter(IQueryable<CodeGeneratorRuleDAO> query, CodeGeneratorRuleFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<CodeGeneratorRuleDAO> initQuery = query.Where(q => false);
            foreach (CodeGeneratorRuleFilter CodeGeneratorRuleFilter in filter.OrFilter)
            {
                IQueryable<CodeGeneratorRuleDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, CodeGeneratorRuleFilter.Id);
                queryable = queryable.Where(q => q.EntityTypeId, CodeGeneratorRuleFilter.EntityTypeId);
                queryable = queryable.Where(q => q.AutoNumberLenth, CodeGeneratorRuleFilter.AutoNumberLenth);
                queryable = queryable.Where(q => q.StatusId, CodeGeneratorRuleFilter.StatusId);
                queryable = queryable.Where(q => q.RowId, CodeGeneratorRuleFilter.RowId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<CodeGeneratorRuleDAO> DynamicOrder(IQueryable<CodeGeneratorRuleDAO> query, CodeGeneratorRuleFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case CodeGeneratorRuleOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case CodeGeneratorRuleOrder.EntityType:
                            query = query.OrderBy(q => q.EntityTypeId);
                            break;
                        case CodeGeneratorRuleOrder.AutoNumberLenth:
                            query = query.OrderBy(q => q.AutoNumberLenth);
                            break;
                        case CodeGeneratorRuleOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case CodeGeneratorRuleOrder.Row:
                            query = query.OrderBy(q => q.RowId);
                            break;
                        case CodeGeneratorRuleOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case CodeGeneratorRuleOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case CodeGeneratorRuleOrder.EntityType:
                            query = query.OrderByDescending(q => q.EntityTypeId);
                            break;
                        case CodeGeneratorRuleOrder.AutoNumberLenth:
                            query = query.OrderByDescending(q => q.AutoNumberLenth);
                            break;
                        case CodeGeneratorRuleOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case CodeGeneratorRuleOrder.Row:
                            query = query.OrderByDescending(q => q.RowId);
                            break;
                        case CodeGeneratorRuleOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<CodeGeneratorRule>> DynamicSelect(IQueryable<CodeGeneratorRuleDAO> query, CodeGeneratorRuleFilter filter)
        {
            List<CodeGeneratorRule> CodeGeneratorRules = await query.Select(q => new CodeGeneratorRule()
            {
                Id = filter.Selects.Contains(CodeGeneratorRuleSelect.Id) ? q.Id : default(long),
                EntityTypeId = filter.Selects.Contains(CodeGeneratorRuleSelect.EntityType) ? q.EntityTypeId : default(long),
                AutoNumberLenth = filter.Selects.Contains(CodeGeneratorRuleSelect.AutoNumberLenth) ? q.AutoNumberLenth : default(long),
                StatusId = filter.Selects.Contains(CodeGeneratorRuleSelect.Status) ? q.StatusId : default(long),
                RowId = filter.Selects.Contains(CodeGeneratorRuleSelect.Row) ? q.RowId : default(Guid),
                Used = filter.Selects.Contains(CodeGeneratorRuleSelect.Used) ? q.Used : default(bool),
                EntityType = filter.Selects.Contains(CodeGeneratorRuleSelect.EntityType) && q.EntityType != null ? new EntityType
                {
                    Id = q.EntityType.Id,
                    Code = q.EntityType.Code,
                    Name = q.EntityType.Name,
                } : null,
                Status = filter.Selects.Contains(CodeGeneratorRuleSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
            }).ToListWithNoLockAsync();

            var Ids = CodeGeneratorRules.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            var CodeGeneratorRuleEntityComponentMappings = await DataContext.CodeGeneratorRuleEntityComponentMapping
                .Where(x => x.CodeGeneratorRuleId, IdFilter)
                .Select(x => new CodeGeneratorRuleEntityComponentMapping
                {
                    CodeGeneratorRuleId = x.CodeGeneratorRuleId,
                    EntityComponentId = x.EntityComponentId,
                    Sequence = x.Sequence,
                    Value = x.Value,
                    EntityComponent = x.EntityComponent == null ? null : new EntityComponent
                    {
                        Id = x.EntityComponent.Id,
                        Code = x.EntityComponent.Code,
                        Name = x.EntityComponent.Name,
                    }
                }).ToListWithNoLockAsync();
            foreach (var CodeGeneratorRule in CodeGeneratorRules)
            {
                CodeGeneratorRule.CodeGeneratorRuleEntityComponentMappings = CodeGeneratorRuleEntityComponentMappings.Where(x => x.CodeGeneratorRuleId == CodeGeneratorRule.Id).ToList();
            }
            return CodeGeneratorRules;
        }

        public async Task<int> Count(CodeGeneratorRuleFilter filter)
        {
            IQueryable<CodeGeneratorRuleDAO> CodeGeneratorRules = DataContext.CodeGeneratorRule.AsNoTracking();
            CodeGeneratorRules = DynamicFilter(CodeGeneratorRules, filter);
            CodeGeneratorRules = OrFilter(CodeGeneratorRules, filter);
            return await CodeGeneratorRules.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(CodeGeneratorRuleFilter filter)
        {
            IQueryable<CodeGeneratorRuleDAO> CodeGeneratorRules = DataContext.CodeGeneratorRule.AsNoTracking();
            CodeGeneratorRules = DynamicFilter(CodeGeneratorRules, filter);
            return await CodeGeneratorRules.CountWithNoLockAsync();
        }

        public async Task<List<CodeGeneratorRule>> List(CodeGeneratorRuleFilter filter)
        {
            if (filter == null) return new List<CodeGeneratorRule>();
            IQueryable<CodeGeneratorRuleDAO> CodeGeneratorRuleDAOs = DataContext.CodeGeneratorRule.AsNoTracking();
            CodeGeneratorRuleDAOs = DynamicFilter(CodeGeneratorRuleDAOs, filter);
            CodeGeneratorRuleDAOs = OrFilter(CodeGeneratorRuleDAOs, filter);
            CodeGeneratorRuleDAOs = DynamicOrder(CodeGeneratorRuleDAOs, filter);
            List<CodeGeneratorRule> CodeGeneratorRules = await DynamicSelect(CodeGeneratorRuleDAOs, filter);
            return CodeGeneratorRules;
        }

        public async Task<List<CodeGeneratorRule>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<CodeGeneratorRule> CodeGeneratorRules = await DataContext.CodeGeneratorRule.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new CodeGeneratorRule()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                EntityTypeId = x.EntityTypeId,
                AutoNumberLenth = x.AutoNumberLenth,
                StatusId = x.StatusId,
                RowId = x.RowId,
                Used = x.Used,
                EntityType = x.EntityType == null ? null : new EntityType
                {
                    Id = x.EntityType.Id,
                    Code = x.EntityType.Code,
                    Name = x.EntityType.Name,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToListWithNoLockAsync();

            List<CodeGeneratorRuleEntityComponentMapping> CodeGeneratorRuleEntityComponentMappings = await DataContext.CodeGeneratorRuleEntityComponentMapping.AsNoTracking()
            .Where(x => x.CodeGeneratorRuleId, IdFilter)
            .Select(x => new CodeGeneratorRuleEntityComponentMapping
            {
                CodeGeneratorRuleId = x.CodeGeneratorRuleId,
                EntityComponentId = x.EntityComponentId,
                Sequence = x.Sequence,
                Value = x.Value,
                EntityComponent = new EntityComponent
                {
                    Id = x.EntityComponent.Id,
                    Code = x.EntityComponent.Code,
                    Name = x.EntityComponent.Name,
                },
            }).ToListWithNoLockAsync();

            foreach (CodeGeneratorRule CodeGeneratorRule in CodeGeneratorRules)
            {
                CodeGeneratorRule.CodeGeneratorRuleEntityComponentMappings = CodeGeneratorRuleEntityComponentMappings.Where(x => x.CodeGeneratorRuleId == CodeGeneratorRule.Id).ToList();
            }

            return CodeGeneratorRules;
        }
        public async Task<CodeGeneratorRule> Get(long Id)
        {
            CodeGeneratorRule CodeGeneratorRule = await DataContext.CodeGeneratorRule.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new CodeGeneratorRule()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                EntityTypeId = x.EntityTypeId,
                AutoNumberLenth = x.AutoNumberLenth,
                StatusId = x.StatusId,
                RowId = x.RowId,
                Used = x.Used,
                EntityType = x.EntityType == null ? null : new EntityType
                {
                    Id = x.EntityType.Id,
                    Code = x.EntityType.Code,
                    Name = x.EntityType.Name,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (CodeGeneratorRule == null)
                return null;

            CodeGeneratorRule.CodeGeneratorRuleEntityComponentMappings = await DataContext.CodeGeneratorRuleEntityComponentMapping.AsNoTracking()
                .Where(x => x.CodeGeneratorRuleId == CodeGeneratorRule.Id)
                .Select(x => new CodeGeneratorRuleEntityComponentMapping
                {
                    CodeGeneratorRuleId = x.CodeGeneratorRuleId,
                    EntityComponentId = x.EntityComponentId,
                    Sequence = x.Sequence,
                    Value = x.Value,
                    EntityComponent = new EntityComponent
                    {
                        Id = x.EntityComponent.Id,
                        Code = x.EntityComponent.Code,
                        Name = x.EntityComponent.Name,
                    },
                }).ToListWithNoLockAsync();

            return CodeGeneratorRule;
        }

        public async Task<bool> BulkMerge(List<CodeGeneratorRule> CodeGeneratorRules)
        {
            List<CodeGeneratorRuleDAO> CodeGeneratorRuleDAOs = new List<CodeGeneratorRuleDAO>();
            List<CodeGeneratorRuleEntityComponentMappingDAO> CodeGeneratorRuleEntityComponentMappingDAOs = new List<CodeGeneratorRuleEntityComponentMappingDAO>();

            foreach (CodeGeneratorRule CodeGeneratorRule in CodeGeneratorRules)
            {
                CodeGeneratorRuleDAO CodeGeneratorRuleDAO = new CodeGeneratorRuleDAO();
                CodeGeneratorRuleDAO.Id = CodeGeneratorRule.Id;
                CodeGeneratorRuleDAO.EntityTypeId = CodeGeneratorRule.EntityTypeId;
                CodeGeneratorRuleDAO.StatusId = CodeGeneratorRule.StatusId;
                CodeGeneratorRuleDAO.CreatedAt = CodeGeneratorRule.CreatedAt;
                CodeGeneratorRuleDAO.UpdatedAt = CodeGeneratorRule.UpdatedAt;
                CodeGeneratorRuleDAO.DeletedAt = CodeGeneratorRule.DeletedAt;
                CodeGeneratorRuleDAO.AutoNumberLenth = CodeGeneratorRule.AutoNumberLenth;
                CodeGeneratorRuleDAO.RowId = CodeGeneratorRule.RowId;
                CodeGeneratorRuleDAO.Used = CodeGeneratorRule.Used;
                CodeGeneratorRuleDAOs.Add(CodeGeneratorRuleDAO);

                foreach (CodeGeneratorRuleEntityComponentMapping CodeGeneratorRuleEntityComponentMapping in CodeGeneratorRule.CodeGeneratorRuleEntityComponentMappings)
                {
                    CodeGeneratorRuleEntityComponentMappingDAO CodeGeneratorRuleEntityComponentMappingDAO = new CodeGeneratorRuleEntityComponentMappingDAO
                    {
                        CodeGeneratorRuleId = CodeGeneratorRule.Id,
                        Sequence = CodeGeneratorRuleEntityComponentMapping.Sequence,
                        Value = CodeGeneratorRuleEntityComponentMapping.Value,
                        EntityComponentId = CodeGeneratorRuleEntityComponentMapping.EntityComponentId,
                    };
                    CodeGeneratorRuleEntityComponentMappingDAOs.Add(CodeGeneratorRuleEntityComponentMappingDAO);
                }
            }
            await DataContext.BulkMergeAsync(CodeGeneratorRuleDAOs);
            await DataContext.BulkMergeAsync(CodeGeneratorRuleEntityComponentMappingDAOs);
            return true;
        }
    }
}

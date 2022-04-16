namespace DMS.Rpc.reports.report_store.report_statistic_problem
{
    public class ReportStatisticProblem_ContentDTO
    {
        public long ProblemTypeId { get; set; }
        public string ProblemTypeName { get; set; }
        public long WaitingCounter { get; set; }
        public long ProcessCounter { get; set; }
        public long CompletedCounter { get; set; }
        public long Total => WaitingCounter + ProcessCounter + CompletedCounter;
    }
}

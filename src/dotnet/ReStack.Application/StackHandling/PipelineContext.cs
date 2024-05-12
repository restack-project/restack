using ReStack.Application.StackHandling.Languages;
using ReStack.Domain.Entities;

namespace ReStack.Application.StackHandling
{
    public class PipelineContext
    {
        public Stack Stack { get; private set; }
        public Job Job { get; private set; }
        public string WorkDirectory { get; private set; }
        public BaseLanguage Strategy { get; private set; }
        public bool StopRequested { get; set; }

        public PipelineContext(Stack stack, Job job, string workDirectory, BaseLanguage strategy)
        {
            Stack = stack;
            Job = job;
            WorkDirectory = workDirectory;
            Strategy = strategy;
        }

        public void AddLog(List<Log> logs)
        {
            foreach (var log in logs)
            {
                AddLog(log);
            }
        }

        public void AddLog(Log log)
        {
            log.JobId = Job.Id;
            Job.Logs.Add(log);

            if (Job.Logs.Any(x => x.Error))
                Job.State = Domain.ValueObjects.JobState.Failed;
        }
    }
}

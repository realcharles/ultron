using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Z.MVC.Core
{
    public class ZTaskManager
    {
        private static IList<ZTask> tasks;
        public static IList<ZTask> Tasks
        {
            get
            {
                if (tasks == null)
                    tasks = new List<ZTask>();
                return tasks;
            }
        }
        public static void Add(ZTask task)
        {
            if (!Tasks.Any(t => t.Name == task.Name))
            {
                Tasks.Add(task);
                task.Debug(EnumLogType.Info, "Add Task:[" + task.Id + "]" + task.Name);
            }
        }
       
        public static void Run(string id)
        {
            var task = Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task != null)
            {
                task.IsManual = false;
                task.Run();
            }
        }
    }
    public abstract class ZTask:IDB
    {
        public string Id
        {
            get
            {
                return this.GetType().Name;
            }
            
        }
        public string Name;
        private BackgroundWorker Worker;
        public EnumTaskStatu Statu;
        public DateTime PreRunTime;
        public bool IsManual;
        public object MonitorEntyty;
        public ZTask(string name)
        {
            Name = name;
            Statu = EnumTaskStatu.Waiting;
            PreRunTime = DateTime.MinValue;
            MonitorEntyty = new object();
            Worker = new BackgroundWorker();
            Worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
        }


        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                System.Threading.Monitor.Enter(MonitorEntyty);
                if (Statu == EnumTaskStatu.Waiting)
                {
                    Statu = EnumTaskStatu.Runing;
                    PreRunTime = DateTime.Now;
                    Begin(e.Argument);
                    Statu = EnumTaskStatu.Done;
                }

            }
            catch (Exception ex)
            {
                Statu = EnumTaskStatu.Waiting;
                ex.Debug(EnumLogType.Error, string.Format("TaskID:{0},TaskName:{1}:{2}", this.Id, this.Name, ex.Message));
            }
            finally
            {
                System.Threading.Monitor.Exit(MonitorEntyty);
            }

        }
        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statu = EnumTaskStatu.Waiting;
            End();
        }
        protected abstract void Begin(object obj = null);
        protected abstract void End();
        public  void Run(object obj = null)
        {
            if (Worker.IsBusy || Statu == EnumTaskStatu.Stoped)
                return;
            Worker.RunWorkerAsync(obj);
        }
    }

    public enum EnumTaskStatu
    {
        Waiting,
        Runing,
        Done,
        Stoped
    }
}

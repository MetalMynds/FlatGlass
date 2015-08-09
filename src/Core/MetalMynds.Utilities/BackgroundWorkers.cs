using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;

namespace MetalMynds.Utilities
{

    public class BackgroundWorkers
    {
        public event EventHandler<DoWorkEventArgs> WorkerWork;
        public event EventHandler<RunWorkerCompletedEventArgs> WorkerCompleted;
        
        protected Dictionary<String, BackgroundWorker> BaseBackgroundWorkers = new Dictionary<string, BackgroundWorker>();

        public BackgroundWorkers()
        {
        }

        

        protected void OnWork(DoWorkEventArgs e)
        {
            if (WorkerWork != null)
            {
                WorkerWork(this, e);
            }
        }

        protected void OnCompleted(RunWorkerCompletedEventArgs e)
        {
            if (WorkerCompleted != null)
            {
                WorkerCompleted(this, e);
            }
        }

        protected void HandleDoWork(object sender, DoWorkEventArgs e)
        {
            OnWork(e);
        }

        protected void HandleCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnCompleted(e);

            WorkerParameters parameters = (WorkerParameters)e.Result;

            Kill(parameters.WorkerID, false);

        }

        public virtual String Start(Object Parameters)
        {
            return CreateNewWorker(Parameters);
        }

        public virtual String StartAndKillAllOthers(Object Parameters)
        {

            KillWorkers();

            return CreateNewWorker(Parameters);
        }

        public virtual void Kill(String WorkerID,Boolean Cancel)
        {
            KillWorker(WorkerID, Cancel);
        }

        public virtual void KillAll()
        {
            KillWorkers();
        }

        protected virtual void KillWorkers()
        {

            for (int count = BaseBackgroundWorkers.Count - 1; count >= 0; count += -1)
            {
                string workerID = BaseBackgroundWorkers.Keys.Last<String>();

                {

                    if (BaseBackgroundWorkers[workerID].CancellationPending)
                    {
                        KillWorker(workerID, false);
                    }


                    if (BaseBackgroundWorkers[workerID].IsBusy)
                    {
                        KillWorker(workerID, true);
                    }

                }

            }

        }


        protected virtual string CreateNewWorker(Object Parameters)
        {

            String newID = System.Guid.NewGuid().ToString("N");

            WorkerParameters parameters = new WorkerParameters(newID, Parameters);
            BackgroundWorker newBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };

            newBackgroundWorker.DoWork += HandleDoWork;
            newBackgroundWorker.RunWorkerCompleted += HandleCompleted;

            BaseBackgroundWorkers.Add(newID, newBackgroundWorker);

            newBackgroundWorker.RunWorkerAsync(parameters);

            return newID;

        }


        protected virtual void KillWorker(string WorkerID, bool Cancel)
        {
            BackgroundWorker backgroundWorker = BaseBackgroundWorkers[WorkerID];
            backgroundWorker.DoWork -= HandleDoWork;
            backgroundWorker.RunWorkerCompleted -= HandleCompleted;
            if (Cancel)
            {
                backgroundWorker.CancelAsync();
            }
            BaseBackgroundWorkers.Remove(WorkerID);

        }

    }
}

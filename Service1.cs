using System.ServiceProcess;
using System.Timers;
using log4net;
using log4net.Config;

namespace WS.RdvPref
{
    public partial class Service1 : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger("Prefecture");
        Timer timer = new Timer();


        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            XmlConfigurator.Configure();

            log.Info("RDV prefcture service starting ...");

            timer.Elapsed += new ElapsedEventHandler(tmrExecutor_Elapsed); 
            timer.Interval = 11*60*1000;
            timer.Enabled = true;
            timer.Start();
        }
        private void tmrExecutor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Do your work here 
            _ = Prefecture.CheckPremierTitre();
            _ = Prefecture.CheckRenouvellement1();
            _ = Prefecture.CheckRenouvellement2();

        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }
    }
}

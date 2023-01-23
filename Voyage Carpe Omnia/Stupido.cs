using Dewdrop.Debugging;
using Dewdrop.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCO
{
    public class Stupido : State
    {
        public override void OnEnter()
        {
            base.OnEnter();
            DBG.Log("enter");
        }

        public override void OnExit()
        {
            base.OnExit();
            DBG.Log("exit");
        }

        public override void Update()
        {
            base.Update();
            DBG.Log("update");
        }

        public override bool CanBeInterrupted()
        {
            DBG.Log("can be interrupted");
            return base.CanBeInterrupted();
        }
    }
}

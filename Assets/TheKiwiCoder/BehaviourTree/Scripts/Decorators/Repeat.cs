using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class Repeat : DecoratorNode {

        public bool restartOnSuccess = true;
        public bool restartOnFailure = false;
        public int repeatCount = 1;
        private int currentCount = 0;

        protected override void OnStart() {
            currentCount = 0;
        }

        protected override void OnStop() {

        }

        protected override State OnUpdate() {
            switch (child.Update()) {
                case State.Running:
                    break;
                case State.Failure:
                    if (restartOnFailure) {
                        return State.Running;
                    } else {
                        return State.Failure;
                    }
                case State.Success:
                    currentCount++;
                    if (currentCount < repeatCount)
                    {
                        child.Abort();
                        return State.Running;
                    }
                    else
                    {
                        if (restartOnSuccess) {
                            currentCount = 0;
                            return State.Running;
                        } else {
                            return State.Success;
                        }
                    }
            }
            return State.Running;
        }
    }

    
}

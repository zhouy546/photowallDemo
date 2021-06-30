using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace jobsystem
{
    public class Example : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        private void DoExample()
        {
            NativeArray<float> resultArray = new NativeArray<float>(1, Allocator.Temp);

            // instantiate
            SimpleJob myJob = new SimpleJob {
                //initialize
                a = 5f,
                result = resultArray
            };

            AnotherJob secondJob = new AnotherJob();
            secondJob.result = resultArray;

            //schedule
            JobHandle handle = myJob.Schedule();
            JobHandle secondhandle = secondJob .Schedule();
            //other tasks to run in Main Thread in parallel

            secondhandle.Complete();

            float resultingValue = resultArray[0];
            Debug.Log("result = " + resultingValue);
            Debug.Log("myjob.a  = " + myJob.a);
            resultArray.Dispose();
        }

        private struct SimpleJob : IJob
        {
            public float a;

            public NativeArray<float> result;

            public void Execute()
            {
                result[0] = a;
                a = 23f;
            }
        }

        private struct AnotherJob : IJob
        {
       
            public NativeArray<float> result;

            public void Execute()
            {
                result[0] = result[0]+1;
            }
        }
    }

}


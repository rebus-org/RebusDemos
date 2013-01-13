using System;
using System.ServiceModel;

namespace CreditAssessment
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CreditAssessment : ICreditAssessment
    {
        readonly Random random = new Random();

        public bool IsOk(string counterpart)
        {
            if (random.Next(4) != 0) throw new WhatamafookException();

            Console.WriteLine("Assessing credit status for {0}", counterpart);

            return counterpart.Length%2 == 0;
        }
    }
}

using System;

namespace CreditAssessment
{
    public class WhatamafookException : ApplicationException
    {
        public WhatamafookException()
            : base("Something went horribly wrong!")
        {

        }
    }
}
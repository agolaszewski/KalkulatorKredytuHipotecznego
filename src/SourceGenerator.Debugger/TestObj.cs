﻿using System;
using Domain;
using TestNamespace;

namespace KalkulatorKredytuHipotecznego.Domain;

public partial record TestObj : ValueObject<TestStruct>
{
    public TestStruct Test(TestStruct value)
    {
        return value;
    }
}
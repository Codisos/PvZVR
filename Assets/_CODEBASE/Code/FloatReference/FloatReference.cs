using System;
using UnityEngine;

[Serializable]
public class FloatReference
{
    public referenceType RefType;
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}

public enum referenceType { Constant, Variable };

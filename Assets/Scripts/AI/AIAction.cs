﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : ScriptableObject
{
    public abstract void Act(AIEntity controller);

	public virtual void Initialize( AIEntity controller ) { }
}

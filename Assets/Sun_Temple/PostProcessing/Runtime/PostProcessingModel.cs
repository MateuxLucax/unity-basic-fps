using System;
using Sun_Temple.PostProcessing.Runtime.Attributes;
using UnityEngine;

namespace Sun_Temple.PostProcessing.Runtime
{
    [Serializable]
    public abstract class PostProcessingModel
    {
        [SerializeField, GetSet("enabled")]
        bool m_Enabled;
        public bool enabled
        {
            get { return m_Enabled; }
            set
            {
                m_Enabled = value;

                if (value)
                    OnValidate();
            }
        }

        public abstract void Reset();

        public virtual void OnValidate()
        {}
    }
}

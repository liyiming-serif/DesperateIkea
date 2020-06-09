using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFXController : MonoBehaviour
{
    private static PFXController m_instance;

    public static PFXController Instance()
    {
        if (m_instance == null)
        {
            m_instance = new PFXController();
        }
        return m_instance;
    }

    public ParticleSystem sparks;
    

    void Awake()
    {
        if (!m_instance)
            m_instance = this;

    }
        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class RetractorAbilityManager : MonoBehaviour
{
    public Collider2D playerCollider;
    public RetractorManager retractorPrefab;

    PairOfRetractorsManager[] retractorPairs = new PairOfRetractorsManager[2];
    int currentPair;

    // User inputs
    Vector3 mousePosition;
    bool setRetractor1 = false;
    bool setRetractor2 = false;
    bool useRetractorInput = false;

    void Start()
    {
        currentPair = 0;
        for(int i = 0; i < retractorPairs.Length; i++)
        {
            retractorPairs[i] = new PairOfRetractorsManager();
        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        useRetractorInput = Input.GetButton("UseRetractor");
        if (Input.GetButtonDown("RemoveRetractor"))
        {
            foreach (PairOfRetractorsManager pair in retractorPairs)
            {
                pair.RemoveRetractors();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            setRetractor1 = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            setRetractor2 = true;
        }
        foreach (PairOfRetractorsManager pair in retractorPairs)
        {
            pair.RenderLine();
        }
    }

    void FixedUpdate()
    {
        if (setRetractor1)
        {
            setRetractor1 = false;
            retractorPairs[currentPair].SetRetractor(retractorPrefab, playerCollider, mousePosition, 1);
        }
        if (setRetractor2)
        {
            setRetractor2 = false;
            retractorPairs[currentPair].SetRetractor(retractorPrefab, playerCollider, mousePosition, 2);
            currentPair++;
            currentPair %= retractorPairs.Length;
        }

        foreach (PairOfRetractorsManager pair in retractorPairs)
        {
            if (useRetractorInput)
            {
                pair.UseRetractors();
            }
            pair.UpdateLineWidth();
        }
    }
}

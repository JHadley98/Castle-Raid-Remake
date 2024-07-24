using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Touch input
        TouchInput();

        //Mouse Input
        OnMouseOver();
    }

    private void TouchInput() // When touching screen destroy grass
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Moved)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                ClearGrass(ray);
            }
        }
    }

    private void OnMouseOver() // While mouse is over grass destroy
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        ClearGrass(ray);
    }

    private void ClearGrass(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 center = hit.transform.position;

            //Take center point from ray and destroy grass in a radius (don't want to remove each individual bit of grass at a time)
            Collider[] hitColliders = Physics.OverlapSphere(center, 1f);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Grass"))
                {
                    Destroy(hitCollider.gameObject);
                }
            }
        }
    }
}

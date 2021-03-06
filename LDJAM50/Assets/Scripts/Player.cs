using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] List<GameObject> objects;

    [SerializeField] Transform cursor;
    [SerializeField] Camera cam;
    [SerializeField] Transform camParent;
    [SerializeField] Transform placementCursor;
    [SerializeField] float zoomSpeed = 1.0f;
    [SerializeField] float zoomMovementMultiplier = 1.0f;
    [SerializeField] float minX = 90.0f;
    [SerializeField] float maxX = 0.0f;
    [SerializeField] float minHeight = 30.0f;
    [SerializeField] float maxHeight = 0.0f;
    [SerializeField] float movementSpeedMouse = 1.0f;
    [SerializeField] float movementSpeedKeys = 1.0f;
    [SerializeField] float orbitSpeedMouse = 10.0f;
    [SerializeField] float orbitSpeedKeys = 10.0f;
    [SerializeField] float rotateCursorMultiplier = 10.0f;
    public float zoomLevel = 0.0f;
    RaycastHit hit;
    Vector3 lastMousePos;
    bool wasCardSelected;

    void Update()
    {
        Vector3 movement = Vector3.zero;
        float orbit = 0.0f;
        Vector3 mouseDelta = lastMousePos - Input.mousePosition;
        lastMousePos = Input.mousePosition;

        if (Hand.Instance.IsCardSelected())
        {
            cursor.Rotate(Input.mouseScrollDelta.y * rotateCursorMultiplier * Vector3.up, Space.World);
        }
        else
        {
            zoomLevel += Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
            zoomLevel = Mathf.Clamp01(zoomLevel);
            if (!Mathf.Approximately(zoomLevel, 0.0f) && !Mathf.Approximately(zoomLevel, 1.0f))
            {
                movement += Vector3.forward * Input.mouseScrollDelta.y * zoomMovementMultiplier * Time.deltaTime;
            }
        }

        Vector3 skyVector = camParent.position + Vector3.up * GlobalData.seaLevel + Vector3.up * 100.0f;
        if (Physics.Raycast(skyVector, Vector3.down, out hit))
        {
            Debug.DrawLine(skyVector, hit.point, Color.green);
            cam.transform.position = hit.point + Vector3.up * Mathf.Lerp(minHeight, maxHeight, zoomLevel);
        }
        else
        {
            Debug.DrawRay(skyVector, Vector3.down, Color.red);
        }
        cam.transform.localEulerAngles = new Vector3(Mathf.Lerp(minX, maxX, zoomLevel), 0.0f, 0.0f);

        movement += new Vector3(Input.GetKey(KeyCode.D) ? 1.0f : Input.GetKey(KeyCode.A) ? -1.0f : 0.0f, 0.0f, Input.GetKey(KeyCode.W) ? 1.0f : Input.GetKey(KeyCode.S) ? -1.0f : 0.0f) * movementSpeedKeys;
        if (Input.GetMouseButton(1))
        {
            movement = (Vector3.forward * mouseDelta.y + Vector3.right * mouseDelta.x) * movementSpeedMouse;
        }
        camParent.localPosition += movement * Time.deltaTime;

        orbit += (Input.GetKey(KeyCode.Q) ? 1.0f : Input.GetKey(KeyCode.E) ? -1.0f : 0.0f) * orbitSpeedKeys;
        if (Input.GetMouseButton(2))
        {
            orbit += orbitSpeedMouse * mouseDelta.x;
        }
        Vector3 orbitPoint = Vector3.zero;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            orbitPoint = hit.point;
        }
        transform.RotateAround(orbitPoint, Vector3.up, orbit * Time.deltaTime);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 99999.0f))
        {
            Debug.DrawLine(cam.transform.position, hit.point, Color.green);
            placementCursor.transform.position = hit.point;
            Vector3 cursorPos = hit.point + Vector3.up * 3.0f;
            cursor.position = cursorPos;
            if (Input.GetMouseButtonDown(0) && wasCardSelected && Hand.Instance.IsCardSelected())
            {
                Hand.Instance.UseSelectedCard(cursorPos, cursor.eulerAngles.y);
                //int random = Random.Range(0, objects.Count);
                //Instantiate(objects[random], cursorPos, objects[random].transform.rotation);
            }
        }
        cursor.gameObject.SetActive(Hand.Instance.IsCardSelected());
        wasCardSelected = Hand.Instance.IsCardSelected();
        GetComponent<AudioSource>().volume = zoomLevel;
		camParent.position = new Vector3(Mathf.Clamp(camParent.position.x, -100f, 100f), camParent.position.y, Mathf.Clamp(camParent.position.z, -100.0f, 100.0f));

    }
}

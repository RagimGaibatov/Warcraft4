using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum SelectionState{
    UnitSelected,
    Frame,
    Other
}

public class Management : MonoBehaviour{
    [SerializeField] private Camera _camera;

    [SerializeField] private SelectableObject howered;

    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();

    [SerializeField] private Image frameImage;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;

    public SelectionState CurrentSelectionState;

    void Update(){
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)){
            if (hit.collider.GetComponent<SelectableCollider>()){
                SelectableObject hitSelectable = hit.collider.GetComponent<SelectableCollider>().SelectableObject;
                if (howered){
                    if (howered != hitSelectable){
                        howered.OnUnhover();
                        howered = hitSelectable;
                        howered.OnHover();
                    }
                }
                else{
                    howered = hitSelectable;
                    howered.OnHover();
                }
            }
            else{
                UnhoverCurrent();
            }
        }
        else{
            UnhoverCurrent();
        }

        if (Input.GetMouseButtonUp(0)){
            if (howered){
                if (Input.GetKey(KeyCode.LeftControl)){
                    if (ListOfSelected.Contains(howered)){
                        Unselect(howered);
                    }
                    else{
                        CurrentSelectionState = SelectionState.UnitSelected;
                        Select(howered);
                    }
                }
                else{
                    UnselectAll();
                    CurrentSelectionState = SelectionState.UnitSelected;
                    Select(howered);
                }
            }
        }

        if (CurrentSelectionState == SelectionState.UnitSelected){
            if (Input.GetMouseButton(1)){
                if (hit.collider.CompareTag("Ground")){
                    int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(ListOfSelected.Count));

                    for (int i = 0; i < ListOfSelected.Count; i++){
                        int row = i / rowNumber;
                        int column = i % rowNumber;

                        Vector3 point = hit.point + new Vector3(column, 0f, row);
                        ListOfSelected[i].WhenClickOnGround(point);
                    }
                }
            }
        }


        // выделение рамкой
        if (Input.GetMouseButtonDown(0)){
            _frameStart = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)){
            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);

            Vector2 size = max - min;

            if (size.magnitude > 5){
                frameImage.enabled = true;
                frameImage.rectTransform.anchoredPosition = min;
                frameImage.rectTransform.sizeDelta = size;

                Rect rect = new Rect(min, size);

                UnselectAll();
                Unit[] allUnits = FindObjectsOfType<Unit>();
                for (int i = 0; i < allUnits.Length; i++){
                    Vector2 screenPosition = _camera.WorldToScreenPoint(allUnits[i].transform.position);
                    if (rect.Contains(screenPosition)){
                        Select(allUnits[i]);
                    }
                }

                CurrentSelectionState = SelectionState.Frame;
            }
        }


        if (Input.GetMouseButtonUp(0)){
            frameImage.enabled = false;

            if (ListOfSelected.Count > 0){
                CurrentSelectionState = SelectionState.UnitSelected;
            }
            else{
                CurrentSelectionState = SelectionState.Other;
            }
        }
    }

    void Select(SelectableObject selectableObject){
        if (!ListOfSelected.Contains(selectableObject)){
            ListOfSelected.Add(selectableObject);

            selectableObject.Select();
        }
    }

    public void Unselect(SelectableObject selectableObject){
        if (ListOfSelected.Contains(selectableObject)){
            ListOfSelected.Remove(selectableObject);

            selectableObject.Unselect();
        }
    }

    void UnselectAll(){
        for (int i = 0; i < ListOfSelected.Count; i++){
            ListOfSelected[i].Unselect();
        }

        ListOfSelected.Clear();
        CurrentSelectionState = SelectionState.Other;
    }

    void UnhoverCurrent(){
        if (howered){
            howered.OnUnhover();
            howered = null;
        }
    }
}
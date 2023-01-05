using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class MenuNavigator : MonoBehaviour
{
    public event Action OnMenuClosed;

    [SerializeField]
    private GameObject BaseMenu;

    [SerializeField]
    private GameObject DefaultSelectable;

    [SerializeField]
    private bool IsDismissable = false;

    private GameObject currentMenu;

    private Stack<Selectable> selectableHistory;
    private Stack<GameObject> menuHistory;


    private void OnEnable()
    {
        ResetMenu();
    }

    public void GoBack()
    {
        if (menuHistory.Count > 0)
        {
            GoToMenu(menuHistory.Pop(), true);
        }
    }

    public void GoToMenu(GameObject menu)
    {
        GoToMenu(menu, false);
    }

    public void GoToMenu(GameObject menu, bool isBack)
    {
        if (menu == null)
        {
            return;
        }

        if (menu.TryGetComponent<INavigatable>(out INavigatable navigatable))
        {
           navigatable.OnNavigatedTo();
        }

        Selectable selectableToSelect = isBack ? selectableHistory.Pop() : GetSelectable(menu);

        if (!isBack)
        {
            selectableHistory.Push(EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>());
            menuHistory.Push(currentMenu);
        }

        EventSystem.current.SetSelectedGameObject(selectableToSelect != null ? selectableToSelect.gameObject : null);
        
        currentMenu.SetActive(false);
        currentMenu = menu;
        currentMenu.SetActive(true);
    }

    public void ResetMenu()
    {
        if (currentMenu != null)
        {
            currentMenu.SetActive(false);
        }
        
        currentMenu = BaseMenu;
        currentMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(DefaultSelectable);
        
        if (currentMenu.TryGetComponent<INavigatable>(out INavigatable navigatable))
        {
            navigatable.OnNavigatedTo();
        }

        selectableHistory = new Stack<Selectable>();
        menuHistory = new Stack<GameObject>();
    }

    public void CloseMenu()
    {
        OnMenuClosed?.Invoke();
        gameObject.SetActive(false);
    }

    private Selectable GetSelectable(GameObject menu)
    {
        Selectable selectable = null;

        if (menu.TryGetComponent<INavigatable>(out INavigatable navigatable))
        {
            selectable = navigatable.FirstSelected();
        }

        return selectable != null ? selectable : menu.GetComponentInChildren<Selectable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (selectableHistory.Count > 0)
            {
                GoBack();
            }

            else if (IsDismissable)
            {
                CloseMenu();
            }
        }

        // If no element is selected and the user tries to navigate via keyboard, reacquire focus.
        // This can happen if the user clicks with the mouse outside of any selectable
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                Selectable selectable = currentMenu.GetComponentInChildren<Selectable>();

                EventSystem.current.SetSelectedGameObject(selectable.gameObject);
            }
        }
    }
}

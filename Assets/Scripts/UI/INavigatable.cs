using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface INavigatable
{
    public Selectable FirstSelected();

    public void OnNavigatedTo();
}

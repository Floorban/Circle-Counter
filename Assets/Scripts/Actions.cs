using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
/*    public static Action OnLevelFinished;
    public static Action OnDamageCaused;*/

    public static Action OnShoot;
    public static Action<Bullet> OnReload;
    public static Action OnSkip;

    public static Action<Bullet> OnBulletSelected;
    public static Action OnBulletDeselected;
    public static Action<Hole> OnHoleSelected;

    public static Action<People> onMouseOverPpl;
    public static Action<People> onClickPpl;
    public static Action onMouseExitPpl;

    public static Action OnLevelStart;
    public static Action OnLevelEnd;

}

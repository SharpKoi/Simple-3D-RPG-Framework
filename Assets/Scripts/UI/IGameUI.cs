using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public interface IGameUI {
    GameObject GetGameObject();

    void TransitionIn();

    TweenerCore<Color, Color, ColorOptions> TransitionOut();
}

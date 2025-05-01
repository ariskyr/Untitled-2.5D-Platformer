using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDirectionProvider
{
    // The only property InteractionPromptUI needs
    bool facingRight { get; }
}

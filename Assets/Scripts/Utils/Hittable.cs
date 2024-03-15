using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Pixelplacement;

public class Hittable : MonoBehaviour
{
    public enum HitType
    {
        None,
        Inflate,
        Push,
        Color
    }

    private static readonly Color DeadColor = new(0.4f, 0.4f, 0.4f);
    private static readonly Color HitColor = new(0.2f, 0.0f, 0.0f);

    public HitType hitType = HitType.None;
    public bool disableHitEffect = false;
    public Transform spriteParent;
    public ParticleSystem customHitEffect;
    public AudioClip customHitSound;
    public bool hideWhenDead = false;
    public Material hitMaterial;

    private SpriteRenderer sprite;
    private float baseScale;
    protected Color defaultColor = Color.white;
    private Material defaultMaterial;

    public event Action<Vector2, Vector2> OnHit;

    protected virtual void Awake()
    {
        //Find all child sprite renderers
        Transform spriteParentTransform = spriteParent != null ? spriteParent : transform;
        sprite = spriteParentTransform.GetComponentInChildren<SpriteRenderer>();

        baseScale = transform.localScale.y;
        defaultMaterial = sprite.material;
    }

    public virtual void OnAttackHit(Vector2 position, Vector2 force, int damage)
    {
        // Damage
        OnHit?.Invoke(position, force);

        // hit behaviour
        if (hitType == HitType.Inflate)
        {
            Tween.LocalScale(transform, new Vector3(transform.localScale.x, baseScale, baseScale),
                new Vector3(transform.localScale.x * 1.01f, baseScale + 0.05f, baseScale), 0.5f, 0,
                Tween.EaseWobble);
        }
        else if (hitType == HitType.Push)
        {
            float hitAmount = 0.05f;
            Tween.Position(transform, transform.position,
                transform.position + new Vector3(UnityEngine.Random.Range(-hitAmount, hitAmount),
                    UnityEngine.Random.Range(-hitAmount, hitAmount), 0),
                0.5f, 0, Tween.EaseWobble);
        }
        else if (hitType == HitType.Color)
        {
            //Impact color flash
            sprite.material = hitMaterial;
            StartCoroutine(ResetMaterial(0.1f));
        }

        // Impact particle effect
        if (!disableHitEffect)
        {
            if (customHitEffect != null)
            {
                EffectManager.Instance.PlayOneShot(customHitEffect, transform.position);
            }
            CameraManager.Instance.CameraShake(0.03f, 0.5f);

            if (customHitEffect != null)
            {
                SoundManager.Instance.PlaySoundAtLocation(customHitSound, transform.position);
            }
        }
    }

    private IEnumerator ResetMaterial(float delay)
    {
        yield return new WaitForSeconds(delay);
        sprite.material = defaultMaterial;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;

public class Piston : Singleton<Piston>
{
    public enum Part
    {
        PIN_CLIP,
        PISTON,
        ROD,
        ROD_BEARING_CAP_SIDE,
        ROD_BEARING_ROD_SIDE,
        ROD_BOLT,
        ROD_CAP,
        WRIST_PIN
    }

    public event Action allPartsAssembled;

    public List<PistonPart> parts;
    public List<PistonPart> assembledParts;

    private void Start()
    {
        LazyLoad();

        // add rod part assembled for default
        PistonPart rodPart = parts.FirstOrDefault(x => x.type is Part.ROD);
        assembledParts.Add(rodPart);

        // subscribe parts assembly and dissambly events
        foreach (var part in parts) 
        {
            part.partAssembled += OnPartAssembled;
            part.partDissambled += OnPartDissambled;
        } 
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        LazyLoad();

        // unsubscribe parts assembly and dissambly events
        foreach (var part in parts)
        {
            part.partAssembled -= OnPartAssembled;
            part.partDissambled -= OnPartDissambled;
        }
    }

    private void OnPartAssembled(PistonPart part)
    {
        if (assembledParts.Contains(part)) return;

        assembledParts.Add(part);

        if (assembledParts.Count == parts.Count)
        {
            allPartsAssembled?.Invoke();
        }
    }

    private void OnPartDissambled(PistonPart part)
    {
        if (assembledParts.Contains(part)) return;

        assembledParts.Remove(part);
    }

    private void LazyLoad()
    {
        if (parts == null) 
        {
            parts = GetComponentsInChildren<PistonPart>().ToList();
        }
    }
}

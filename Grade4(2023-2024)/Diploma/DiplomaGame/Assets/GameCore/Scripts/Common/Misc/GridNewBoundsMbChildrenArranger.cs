using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using GameBasicsCore.Game.Tiny.Arrange.Children.BoundsMbs;
using UnityEngine;

namespace GameCore.Common.Misc
{
    internal class GridNewBoundsMbChildrenArranger : BoundsMbChildrenArranger
    {
        [SerializeField] private bool _autoSize = true;
        [SerializeField, HideIf("IsAutoSize")] private float _sizeX;
        [SerializeField, HideIf("IsAutoSize")] private float _sizeY;
        [SerializeField, HideIf("IsAutoSize")] private float _sizeZ;
        [SerializeField] private int _rows = 1;
        [SerializeField] private int _cols = 1;
        // [SerializeField] private bool _alignCenterX;
        [SerializeField] private bool _alignEdgeX;
        [SerializeField] private bool _alignEdgeZ;
        [SerializeField] private float _axisZ = 1;

        protected override List<Vector3> GetChildrenPositionsInternal()
        {
            var firstBound = _bounds.FirstOrDefault();
            var sX = _autoSize ? firstBound.size.x : _sizeX;
            //var sY = _autoSize ? firstBound.size.y : _sizeY;
            var sZ = _autoSize ? firstBound.size.z : _sizeZ;
            
            var prevEdgeX = _start.x;
            var prevEdgeY = _start.y;
            var prevEdgeZ = _start.z;
            var row = 0;
            var col = 0;
            int count = 0;
            //int totalRows = 0;
            for (int i = 0; i < _bounds.Count; i++)
            {
                var b = _bounds[i];
                var offsetX = col * sX + _gap.x * col /*+ _start.x*/ + (_alignEdgeX ? sX * 0.5f : 0);
                var offsetY = -b.center.y + b.size.y * 0.5f;
                var offsetZ = (row * sZ + _gap.z * row /*+ _start.z*/ + (_alignEdgeZ ? sZ * 0.5f : 0));// * _axisZ;
                var pos = new Vector3(prevEdgeX + offsetX, prevEdgeY + offsetY, (prevEdgeZ + offsetZ) * _axisZ);
                _resultPositions.Add(pos);
                //prevEdgeZ += b.size.z + _gap.z;
                prevEdgeX += b.size.x + _gap.x;

                count++;
                if (count >= _cols)
                {
                    count = 0;
                    prevEdgeX = _start.x;
                    row++;
                    if (row >= _rows)
                    {
                        row = 0;
                        col = 0;
                        prevEdgeY += b.size.y + _gap.y;
                    }
                }
            }

            /*if (_alignCenterX)
            {
                if (totalRows < row + 1) totalRows = row + 1;
                var boundsX = totalRows * sX + _gap.x * (totalRows - 1);
                for (int i = 0; i < _bounds.Count; i++)
                {
                    var pos = _result[i];
                    pos.x += -boundsX * 0.5f + sX * 0.5f;
                    _result[i] = pos;
                }
            }*/
            return _resultPositions;
        }

        private bool IsAutoSize() => _autoSize;
    }
}
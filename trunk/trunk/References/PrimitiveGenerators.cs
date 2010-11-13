using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace ThingReferences
{
    public class PrimitiveGenerators
    {
        unsafe static public void CreateSphere(string strName, float r, int nRings, int nSegments)
        {

            MeshPtr pSphere = MeshManager.Singleton.CreateManual(strName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            SubMesh pSphereVertex = pSphere.CreateSubMesh();

            pSphere.sharedVertexData = new VertexData();
            VertexData vertexData = pSphere.sharedVertexData;

            // define the vertex format
            VertexDeclaration vertexDecl = vertexData.vertexDeclaration;
            uint currOffset = 0;
            // positions
            vertexDecl.AddElement(0, currOffset, VertexElementType.VET_FLOAT3, VertexElementSemantic.VES_POSITION);
            currOffset += VertexElement.GetTypeSize(VertexElementType.VET_FLOAT3);
            // normals
            vertexDecl.AddElement(0, currOffset, VertexElementType.VET_FLOAT3, VertexElementSemantic.VES_NORMAL);
            currOffset += VertexElement.GetTypeSize(VertexElementType.VET_FLOAT3);
            // two dimensional texture coordinates
            vertexDecl.AddElement(0, currOffset, VertexElementType.VET_FLOAT2, VertexElementSemantic.VES_TEXTURE_COORDINATES, 0);
            currOffset += VertexElement.GetTypeSize(VertexElementType.VET_FLOAT2);

            // allocate the vertex buffer
            vertexData.vertexCount = (uint)((nRings + 1) * (nSegments + 1));
            HardwareVertexBufferSharedPtr vBuf = HardwareBufferManager.Singleton.CreateVertexBuffer(vertexDecl.GetVertexSize(0), vertexData.vertexCount, HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY, false);
            VertexBufferBinding binding = vertexData.vertexBufferBinding;
            binding.SetBinding(0, vBuf);
            float* pVertex = (float*)vBuf.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);

            // allocate index buffer
            pSphereVertex.indexData.indexCount = (uint)(6 * nRings * (nSegments + 1));
            pSphereVertex.indexData.indexBuffer = HardwareBufferManager.Singleton.CreateIndexBuffer(HardwareIndexBuffer.IndexType.IT_16BIT, pSphereVertex.indexData.indexCount, HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY, false);
            HardwareIndexBufferSharedPtr iBuf = pSphereVertex.indexData.indexBuffer;
            ushort* pIndices = (ushort*)iBuf.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);

            float fDeltaRingAngle = (float)(Mogre.Math.PI / nRings);
            float fDeltaSegAngle = (float)(2 * Mogre.Math.PI / nSegments);
            ushort wVerticeIndex = 0;

            // Generate the group of rings for the sphere
            for (int ring = 0; ring <= nRings; ring++)
            {
                float r0 = r * Mogre.Math.Sin(ring * fDeltaRingAngle);
                float y0 = r * Mogre.Math.Cos(ring * fDeltaRingAngle);

                // Generate the group of segments for the current ring
                for (int seg = 0; seg <= nSegments; seg++)
                {
                    float x0 = r0 * Mogre.Math.Sin(seg * fDeltaSegAngle);
                    float z0 = r0 * Mogre.Math.Cos(seg * fDeltaSegAngle);

                    // Add one vertex to the strip which makes up the sphere
                    *pVertex++ = x0;
                    *pVertex++ = y0;
                    *pVertex++ = z0;

                    Mogre.Vector3 vNormal = (new Mogre.Vector3(x0, y0, z0)).NormalisedCopy;
                    *pVertex++ = vNormal.x;
                    *pVertex++ = vNormal.y;
                    *pVertex++ = vNormal.z;

                    *pVertex++ = (float)seg / (float)nSegments;
                    *pVertex++ = (float)ring / (float)nRings;

                    if (ring != nRings)
                    {
                        // each vertex (except the last) has six indices pointing to it
                        *pIndices++ = (ushort)(wVerticeIndex + nSegments + 1);
                        *pIndices++ = wVerticeIndex;
                        *pIndices++ = (ushort)(wVerticeIndex + nSegments);
                        *pIndices++ = (ushort)(wVerticeIndex + nSegments + 1);
                        *pIndices++ = (ushort)(wVerticeIndex + 1);
                        *pIndices++ = wVerticeIndex;
                        wVerticeIndex++;
                    }
                }; // end for seg
            } // end for ring

            // Unlock
            vBuf.Unlock();
            iBuf.Unlock();

            // Generate face list
            pSphereVertex.useSharedVertices = true;

            // the original code was missing this line:
            pSphere._setBounds(new AxisAlignedBox(new Mogre.Vector3(-r, -r, -r), new Mogre.Vector3(r, r, r)), false);
            pSphere._setBoundingSphereRadius(r);

            // this line makes clear the mesh is loaded (avoids memory leaks)
            pSphere.Load();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace ExtraMegaBlob.References
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

        /// <summary>
        /// Create a tetrahedron with point of origin in middle of volume. 
        /// It will be added to the SceneManager as ManualObject. The material must still exists.
        /// </summary>
        /// <param name="position">Position in scene</param>
        /// <param name="scale">Size of the tetrahedron</param>
        /// <param name="name">Name of the ManualObject that will be created</param>
        /// <param name="materialName">Name of the used material</param>
        ///
        //        /// Usage example

        //// a material with the name materialName must be created or loaded!

        //Vector3 position = new Vector3(1, 1, -1);
        //Single size = 1;
        //String tetraName = "tetra";

        //// create manual object
        //CreateTetrahedron(tetraName, position, size, materialName);

        //// attach to scene
        //mScene.Smgr.RootSceneNode.AttachObject(mScene.Smgr.GetManualObject(tetraName));

        //// remove tetrahedrons
        //if (mScene.Smgr.HasManualObject(tetraName))
        //    mScene.Smgr.GetManualObject(tetraName).Clear();
        public static ManualObject CreateTetrahedron(String name, Mogre.Vector3 position, Single scale, String materialName)
        {
            ManualObject manObTetra = new ManualObject(name);
            manObTetra.CastShadows = false;

            // render just before overlays (so all objects behind the transparent tetrahedron are visible)
            manObTetra.RenderQueueGroup = (byte)RenderQueueGroupID.RENDER_QUEUE_OVERLAY - 1; // = 99

            Mogre.Vector3[] c = new Mogre.Vector3[4]; // corners

            // calculate corners of tetrahedron (with point of origin in middle of volume)
            Single mbot = scale * 0.2f;      // distance middle to bottom
            Single mtop = scale * 0.62f;     // distance middle to top    
            Single mf = scale * 0.289f;    // distance middle to front
            Single mb = scale * 0.577f;    // distance middle to back
            Single mlr = scale * 0.5f;      // distance middle to left right 
            //               width / height / depth
            c[0] = new Mogre.Vector3(-mlr, -mbot, mf);  // left bottom front
            c[1] = new Mogre.Vector3(mlr, -mbot, mf);  // right bottom front
            c[2] = new Mogre.Vector3(0, -mbot, -mb);  // (middle) bottom back
            c[3] = new Mogre.Vector3(0, mtop, 0);  // (middle) top (middle)

            // add position offset for all corners (move tetrahedron)
            for (Int16 i = 0; i <= 3; i++)
                c[i] += position;

            // create bottom
            manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            manObTetra.Position(c[2]);
            manObTetra.Position(c[1]);
            manObTetra.Position(c[0]);
            manObTetra.Triangle(0, 1, 2);
            manObTetra.End();
            // create right back side
            manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            manObTetra.Position(c[1]);
            manObTetra.Position(c[2]);
            manObTetra.Position(c[3]);
            manObTetra.Triangle(0, 1, 2);
            manObTetra.End();
            // create left back side
            manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            manObTetra.Position(c[3]);
            manObTetra.Position(c[2]);
            manObTetra.Position(c[0]);
            manObTetra.Triangle(0, 1, 2);
            manObTetra.End();
            // create front side
            manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            manObTetra.Position(c[0]);
            manObTetra.Position(c[1]);
            manObTetra.Position(c[3]);
            manObTetra.Triangle(0, 1, 2);
            manObTetra.End();
            return manObTetra;

        } // CreateTetrahedron

        unsafe static public void CreateTetrahedron2(string strName)
        {
            float r = 1f;
            int nRings = 8;
            int nSegments = 8;
            Single scale = 1;
            Mogre.Vector3 position = new Mogre.Vector3();
            MeshPtr pTetra = MeshManager.Singleton.CreateManual(strName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            SubMesh pTetraVertex = pTetra.CreateSubMesh();

            pTetra.sharedVertexData = new VertexData();
            VertexData vertexData = pTetra.sharedVertexData;

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
            vertexData.vertexCount = 4; //(uint)((nRings + 1) * (nSegments + 1));
            HardwareVertexBufferSharedPtr vBuf = HardwareBufferManager.Singleton.CreateVertexBuffer(vertexDecl.GetVertexSize(0), vertexData.vertexCount, HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY, false);
            VertexBufferBinding binding = vertexData.vertexBufferBinding;
            binding.SetBinding(0, vBuf);
            float* pVertex = (float*)vBuf.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);


            uint numIndexes = (uint)(6 * 4);

            // allocate index buffer
            pTetraVertex.indexData.indexCount = numIndexes;
            pTetraVertex.indexData.indexBuffer = HardwareBufferManager.Singleton.CreateIndexBuffer(HardwareIndexBuffer.IndexType.IT_16BIT, pTetraVertex.indexData.indexCount, HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY, false);
            HardwareIndexBufferSharedPtr iBuf = pTetraVertex.indexData.indexBuffer;
            ushort* pIndices = (ushort*)iBuf.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);

            //float fDeltaRingAngle = (float)(Mogre.Math.PI / nRings);
            //float fDeltaSegAngle = (float)(2 * Mogre.Math.PI / nSegments);
            ushort wVerticeIndex = 0;

            // calculate corners of tetrahedron (with point of origin in middle of volume)
            Single mbot = scale * 0.2f;      // distance middle to bottom
            Single mtop = scale * 0.62f;     // distance middle to top    
            Single mf = scale * 0.289f;    // distance middle to front
            Single mb = scale * 0.577f;    // distance middle to back
            Single mlr = scale * 0.5f;      // distance middle to left right 
            //               width / height / depth

            Mogre.Vector3[] c = new Mogre.Vector3[4]; // corners

            c[0] = new Mogre.Vector3(-mlr, -mbot, mf);  // left bottom front
            c[1] = new Mogre.Vector3(mlr, -mbot, mf);  // right bottom front
            c[2] = new Mogre.Vector3(0, -mbot, -mb);  // (middle) bottom back
            c[3] = new Mogre.Vector3(0, mtop, 0);  // (middle) top (middle)

            // add position offset for all corners (move tetrahedron)
            for (Int16 i = 0; i <= 3; i++)
                c[i] += position;

            Mogre.Vector3 vNormal = new Mogre.Vector3();

            *pVertex++ = c[0].x;
            *pVertex++ = c[0].y;
            *pVertex++ = c[0].z;
            vNormal = (new Mogre.Vector3(c[0].x, c[0].y, c[0].z)).NormalisedCopy;
            *pVertex++ = vNormal.x;
            *pVertex++ = vNormal.y;
            *pVertex++ = vNormal.z;
            *pVertex++ = (float).25;
            *pVertex++ = (float).25;
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes + 1);
            *pIndices++ = wVerticeIndex;
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes);
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes + 1);
            *pIndices++ = (ushort)(wVerticeIndex + 1);
            *pIndices++ = wVerticeIndex;
            wVerticeIndex++;

            *pVertex++ = c[1].x;
            *pVertex++ = c[1].y;
            *pVertex++ = c[1].z;
            vNormal = (new Mogre.Vector3(c[1].x, c[1].y, c[1].z)).NormalisedCopy;
            *pVertex++ = vNormal.x;
            *pVertex++ = vNormal.y;
            *pVertex++ = vNormal.z;
            *pVertex++ = (float).50;
            *pVertex++ = (float).50;
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes + 1);
            *pIndices++ = wVerticeIndex;
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes);
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes + 1);
            *pIndices++ = (ushort)(wVerticeIndex + 1);
            *pIndices++ = wVerticeIndex;
            wVerticeIndex++;

            *pVertex++ = c[2].x;
            *pVertex++ = c[2].y;
            *pVertex++ = c[2].z;
            vNormal = (new Mogre.Vector3(c[2].x, c[2].y, c[2].z)).NormalisedCopy;
            *pVertex++ = vNormal.x;
            *pVertex++ = vNormal.y;
            *pVertex++ = vNormal.z;
            *pVertex++ = (float).75;
            *pVertex++ = (float).75;
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes + 1);
            *pIndices++ = wVerticeIndex;
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes);
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes + 1);
            *pIndices++ = (ushort)(wVerticeIndex + 1);
            *pIndices++ = wVerticeIndex;
            wVerticeIndex++;

            *pVertex++ = c[3].x;
            *pVertex++ = c[3].y;
            *pVertex++ = c[3].z;
            vNormal = (new Mogre.Vector3(c[3].x, c[3].y, c[3].z)).NormalisedCopy;
            *pVertex++ = vNormal.x;
            *pVertex++ = vNormal.y;
            *pVertex++ = vNormal.z;
            *pVertex++ = (float)1;
            *pVertex++ = (float)1;
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes + 1);
            *pIndices++ = wVerticeIndex;
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes);
            *pIndices++ = (ushort)(wVerticeIndex + numIndexes + 1);
            *pIndices++ = (ushort)(wVerticeIndex + 1);
            *pIndices++ = wVerticeIndex;
            wVerticeIndex++;


            //manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            //manObTetra.Position(c[2]);
            //manObTetra.Position(c[1]);
            //manObTetra.Position(c[0]);
            //manObTetra.Triangle(0, 1, 2);
            //manObTetra.End();
            //// create right back side
            //manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            //manObTetra.Position(c[1]);
            //manObTetra.Position(c[2]);
            //manObTetra.Position(c[3]);
            //manObTetra.Triangle(0, 1, 2);
            //manObTetra.End();
            //// create left back side
            //manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            //manObTetra.Position(c[3]);
            //manObTetra.Position(c[2]);
            //manObTetra.Position(c[0]);
            //manObTetra.Triangle(0, 1, 2);
            //manObTetra.End();
            //// create front side
            //manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            //manObTetra.Position(c[0]);
            //manObTetra.Position(c[1]);
            //manObTetra.Position(c[3]);
            //manObTetra.Triangle(0, 1, 2);
            //manObTetra.End();
            //return manObTetra;



            //// Generate the group of rings for the sphere
            //for (int ring = 0; ring <= nRings; ring++)
            //{
            //    float r0 = r * Mogre.Math.Sin(ring * fDeltaRingAngle);
            //    float y0 = r * Mogre.Math.Cos(ring * fDeltaRingAngle);

            //    // Generate the group of segments for the current ring
            //    for (int seg = 0; seg <= nSegments; seg++)
            //    {
            //        float x0 = r0 * Mogre.Math.Sin(seg * fDeltaSegAngle);
            //        float z0 = r0 * Mogre.Math.Cos(seg * fDeltaSegAngle);

            //        // Add one vertex to the strip which makes up the sphere
            //        *pVertex++ = x0;
            //        *pVertex++ = y0;
            //        *pVertex++ = z0;

            //        Mogre.Vector3 vNormal = (new Mogre.Vector3(x0, y0, z0)).NormalisedCopy;
            //        *pVertex++ = vNormal.x;
            //        *pVertex++ = vNormal.y;
            //        *pVertex++ = vNormal.z;

            //        *pVertex++ = (float)seg / (float)nSegments;
            //        *pVertex++ = (float)ring / (float)nRings;

            //        if (ring != nRings)
            //        {
            //            // each vertex (except the last) has six indices pointing to it
            //            *pIndices++ = (ushort)(wVerticeIndex + nSegments + 1);
            //            *pIndices++ = wVerticeIndex;
            //            *pIndices++ = (ushort)(wVerticeIndex + nSegments);
            //            *pIndices++ = (ushort)(wVerticeIndex + nSegments + 1);
            //            *pIndices++ = (ushort)(wVerticeIndex + 1);
            //            *pIndices++ = wVerticeIndex;
            //            wVerticeIndex++;
            //        }
            //    }; // end for seg
            //} // end for ring

            // Unlock
            vBuf.Unlock();
            iBuf.Unlock();

            // Generate face list
            pTetraVertex.useSharedVertices = true;

            // the original code was missing this line:
            pTetra._setBounds(new AxisAlignedBox(new Mogre.Vector3(-r, -r, -r), new Mogre.Vector3(r, r, r)), false);
            pTetra._setBoundingSphereRadius(r);

            // this line makes clear the mesh is loaded (avoids memory leaks)
            pTetra.Load();
        }

        public static ManualObject CreateTetrahedron3(String name, Mogre.Vector3 position, Single scale, String materialName)
        {
            ManualObject manObTetra = new ManualObject(name);
            manObTetra.CastShadows = false;

            // render just before overlays (so all objects behind the transparent tetrahedron are visible)
            manObTetra.RenderQueueGroup = (byte)RenderQueueGroupID.RENDER_QUEUE_OVERLAY - 1; // = 99

            Mogre.Vector3[] c = new Mogre.Vector3[4]; // corners

            // calculate corners of tetrahedron (with point of origin in middle of volume)
            Single mbot = scale * 0.2f;      // distance middle to bottom
            Single mtop = scale * 0.62f;     // distance middle to top    
            Single mf = scale * 0.289f;    // distance middle to front
            Single mb = scale * 0.577f;    // distance middle to back
            Single mlr = scale * 0.5f;      // distance middle to left right 
            //               width / height / depth
            c[0] = new Mogre.Vector3(-mlr, -mbot, mf);  // left bottom front
            c[1] = new Mogre.Vector3(mlr, -mbot, mf);  // right bottom front
            c[2] = new Mogre.Vector3(0, -mbot, -mb);  // (middle) bottom back
            c[3] = new Mogre.Vector3(0, mtop, 0);  // (middle) top (middle)

            // add position offset for all corners (move tetrahedron)
            for (Int16 i = 0; i <= 3; i++)
                c[i] += position;

            // create bottom
            manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            manObTetra.Position(c[2]);
            manObTetra.Position(c[1]);
            manObTetra.Position(c[0]);
            manObTetra.Triangle(0, 1, 2);
            manObTetra.End();
            // create right back side
            manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            manObTetra.Position(c[1]);
            manObTetra.Position(c[2]);
            manObTetra.Position(c[3]);
            manObTetra.Triangle(0, 1, 2);
            manObTetra.End();
            // create left back side
            manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            manObTetra.Position(c[3]);
            manObTetra.Position(c[2]);
            manObTetra.Position(c[0]);
            manObTetra.Triangle(0, 1, 2);
            manObTetra.End();
            // create front side
            manObTetra.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            manObTetra.Position(c[0]);
            manObTetra.Position(c[1]);
            manObTetra.Position(c[3]);
            manObTetra.Triangle(0, 1, 2);
            manObTetra.End();
            return manObTetra;

        } // CreateTetrahedron

        public static SceneNode CreateTestTetrahedron(SceneManager sm, string name, float scale, Mogre.Vector3 position, String materialName)
        {

            var mo = sm.CreateManualObject(name);
            mo.CastShadows = true;
            var node = sm.RootSceneNode.CreateChildSceneNode();

            Mogre.Vector3[] c = new Mogre.Vector3[4]; // corners

            // calculate corners of tetrahedron (with point of origin in middle of volume)
            Single mbot = scale * 0.2f;      // distance middle to bottom
            Single mtop = scale * 0.62f;     // distance middle to top   
            Single mf = scale * 0.289f;    // distance middle to front
            Single mb = scale * 0.577f;    // distance middle to back
            Single mlr = scale * 0.5f;      // distance middle to left right
            //               width / hight / depth
            c[0] = new Mogre.Vector3(-mlr, -mbot, mf);  // left bottom front
            c[1] = new Mogre.Vector3(mlr, -mbot, mf);  // right bottom front
            c[2] = new Mogre.Vector3(0, -mbot, -mb);  // (middle) bottom back
            c[3] = new Mogre.Vector3(0, mtop, 0);  // (middle) top (middle)

            // add position offset for all corners (move tetrahedron)
            for (Int16 i = 0; i <= 3; i++)
                c[i] += position;

            // create bottom
            mo.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            var normal = (c[1] - c[0]).CrossProduct(c[1] - c[2]).NormalisedCopy;
            var normalA =  normal;
            var normalB =  normal;
            var normalC =  normal;

           

            //this is an attempt to try to make it seem softer.
            normalA.x += 2f;
            normalA.y += 0.5f;
            normalB.y -= 2f;
            normalB.x -= 0.5f;

            mo.Position(c[0]);
            mo.Normal(normalA);
            mo.TextureCoord(0,0);

            mo.Position(c[1]);
            mo.Normal(normalB);
            mo.TextureCoord(0,1);

            mo.Position(c[2]);
            mo.Normal(normalC);
            mo.TextureCoord(1,1);

            mo.Triangle(0, 1, 2);
            mo.Triangle(0, 2, 1);
            mo.End();

            float lineLen = scale/3;



            // create right back side
            mo.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            normal = (c[1] - c[2]).CrossProduct(c[1] - c[3]).NormalisedCopy;
            normalA = normal;
            normalB = normal;
            normalC = normal;

            normalA.x += 2f;
            normalA.y += 0.5f;
            normalB.y -= 2f;
            normalB.x -= 0.5f;

            mo.Position(c[1]);
            mo.Normal(normalA);
            mo.Position(c[2]);
            mo.Normal(normalB);
            mo.Position(c[3]);
            mo.Normal(normalC);
            mo.Triangle(0, 1, 2);
            mo.Triangle(0, 2, 1);
            mo.End();


            // create left back side
            normal = (c[0] - c[2]).CrossProduct(c[2] - c[3]);
            mo.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            mo.Position(c[3]);
            mo.Normal(normal);
            mo.Position(c[2]);
            mo.Normal(normal);
            mo.Position(c[0]);
            mo.Normal(normal);
            mo.Triangle(0, 1, 2);
            mo.Triangle(0, 2, 1);
            mo.End();


            // create front side
            normal = (c[1] - c[3]).CrossProduct(c[0] - c[3]);
            mo.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            mo.Position(c[0]);
            mo.Normal(normal);
            mo.Position(c[1]);
            mo.Normal(normal);
            mo.Position(c[3]);
            mo.Normal(normal);
            mo.Triangle(0, 1, 2);
            mo.Triangle(0, 2, 1);
            mo.End();

            node.AttachObject(mo);
            return node;


        }

        //public MeshPtr CreateMesh(Vertex[] vertexList, Index[] indexList, string meshName, string materialName)
        //{
        //    ManualObject mo = sceneManager.CreateManualObject(meshName + "ManualObject");
        //    mo.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST); //Set the material and state that you are making a mesh out of triangles

        //    //Create all the vertices
        //    foreach (Vertex v in vertexList)
        //    {
        //        mo.Position(v.Position);
        //        mo.Normal(v.Normal);
        //        mo.Colour(v.Color); //NB Ogre uses British English spelling of Colour.
        //        mo.TextureCoord(v.textureCoord.U, v.textureCoord.V);

        //    }

        //    //Create the triangles
        //    foreach (Index index in indexList)
        //    {
        //        mo.Triangle(index.pos1, index.pos2, index.pos3);
        //    }

        //    mo.End(); // End creating the manual object.

        //    return mo.ConvertToMesh(meshName); //Create an actual mesh from the ManualObject, and return.

        //}
    }
}

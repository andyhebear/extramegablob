/*
--------------------------------------------------------------------------------
This source file is part of MHydrax.
Visit ---

Copyright (C) 2009 Christian Haettig

This program is free software; you can redistribute it and/or modify it under
the terms of the GNU Lesser General Public License as published by the Free Software
Foundation; either version 3 of the License, or (at your option) any later
version.

This program is distributed in the hope that it will be useful, but WITHOUT
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with
this program; if not, write to the Free Software Foundation, Inc., 59 Temple
Place - Suite 330, Boston, MA 02111-1307, USA, or go to
http://www.gnu.org/copyleft/lesser.txt.
--------------------------------------------------------------------------------
*/

// MMesh.h

#pragma once

namespace MHydrax {

	/// <summary>
	/// Mesh vertex type enum.
	/// </summary>
	public enum class MVertexType
	{
		VT_POS_NORM_UV,
		VT_POS_NORM,
		VT_POS_UV,
		VT_POS,
	};

	/// <summary>
	/// Class wich contains all funtions/variables related to Hydrax water mesh.
	/// </summary>
	public ref class MMesh {
	private:
		Hydrax::Mesh* internalMesh;

		// Track whether this instance owns, or does not own the reference to the native pointer.
		bool ownsNativeRef;

		// Track whether Dispose has been called.
		bool disposed;

	internal:
		/// <summary>
		/// Creates a new instance of MMesh.
		/// </summary>
		///	<param name="nativeMesh">A native Hydrax::Mesh.</param>
		MMesh(Hydrax::Mesh* nativeMesh) {
			internalMesh = nativeMesh;
			ownsNativeRef = false;
		}

	public:
		/// <summary>
		/// Base Hydrax mesh options.
		/// </summary>
		ref struct MOptions {
		private:
			Hydrax::Mesh::Options* internalOptions;

			// Track whether this instance owns, or does not own the reference to the native pointer.
			bool ownsNativeRef;

			// Track whether Dispose has been called.
			bool disposed;

		internal:
			/// <summary>
			/// Creates a new instance of MMesh.MOptions.
			/// </summary>
			/// <param name="nativeOptions">Native Hydrax::Mesh::Options.</param>
			MOptions(Hydrax::Mesh::Options* nativeOptions) {
				internalOptions = nativeOptions;
				ownsNativeRef = false;
			}

		public:
			/// <summary>
			/// Conversion operator to type Hydrax::Mesh::Options*.
			/// </summary>
			operator Hydrax::Mesh::Options *() {
				if(!disposed) {
					return internalOptions;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

			/// <summary>
			/// Mesh complexity.
			/// </summary>
			/// <returns>The mesh complexity.</returns>
			property int MeshComplexity {
				int get() {
					if(!disposed) {
						return internalOptions->MeshComplexity;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}
			/// <summary>
			/// Grid size (X/Z) world space.
			/// </summary>
			/// <returns>The mesh size.</returns>
			property MSize^ MeshSize {
				MSize^ get() {
					if(!disposed) {
						return gcnew MSize(internalOptions->MeshSize);
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}
			/// <summary>
			/// Water strength(Y axis multiplier).
			/// </summary>
			/// <returns>The mesh strength.</returns>
			property float MeshStrength {
				float get() {
					if(!disposed) {
						return internalOptions->MeshStrength;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}
			/// <summary>
			/// Vertex type.
			/// </summary>
			/// <returns>The vertex type.</returns>
			property MVertexType VertexType {
				MVertexType get() {
					if(!disposed) {
						return (MVertexType)internalOptions->MeshVertexType;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Destructor.
			/// </summary>
			~MOptions() { // implements IDisposable
				if(!disposed) {
					// dispose managed resources

					this->!MOptions();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

		protected:
			/// <summary>
			/// Finalizer.
			/// </summary>
			!MOptions() {
				// dispose unmanaged resources
				if(ownsNativeRef) {
					delete internalOptions;
				}

				disposed = true;
			}
		};

		/// <summary>
		/// Gets current options.
		/// </summary>
		/// <returns>Mesh options.</returns>
		property MMesh::MOptions^ Options {
			MMesh::MOptions^ get() {
				if(!disposed) {
					return gcnew MMesh::MOptions((Hydrax::Mesh::Options*)&internalMesh->getOptions());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			/// <param name="value">Mesh options.</param>
			//void set(MMesh::MOptions^ value) {
			//	if(!disposed) {
			//		internalMesh->setOptions((Hydrax::Mesh::Options&)value);
			//	} else {
			//		throw gcnew System::ObjectDisposedException(this->ToString());
			//	}
			//}
		}

		/// <summary>
		/// Create our water mesh, geometry, entity, etc...
		/// </summary>
		/// <remarks>Call it after setMeshOptions() and setMaterialName()</remarks>
		void Create() {
			if(!disposed) {
				internalMesh->create();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Remove all resources.
		/// </summary>
		void Remove() {
			if(!disposed) {
				internalMesh->remove();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Update geomtry.
		/// </summary>
		/// <param name="numVer">Number of vertices.</param>
		/// <param name="verArray">Vertices array.</param>
		/// <returns>False If number of vertices do not correspond.</returns>
		bool UpdateGeometry(int numVer, void* verArray) {
			if(!disposed) {
				return internalMesh->updateGeometry(numVer, verArray);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Get if a Position point is inside the grid.
		/// </summary>
		/// <param name="position">World-space point.</param>
		/// <returns>True, if position point is inside the grid. Otherwise False.</returns>
		bool IsPointInGrid(Mogre::Vector2 position) {
			if(!disposed) {
				return internalMesh->isPointInGrid((Ogre::Vector2&)position);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Get the [0,1] range x/y grid position from a 2D world space x/z point.
		/// </summary>
		/// <param name="position">World-space point.</param>
		/// <returns>(-1,-1) if the point isn't in the grid.</returns>
		Mogre::Vector2 GetGridPosition(Mogre::Vector2 position) {
			if(!disposed) {
				return GetManagedVector2(internalMesh->getGridPosition((Ogre::Vector2&)position));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Get the object-space position from world-space position.
		/// </summary>
		/// <param name="worldSpacePosition">Position in world coords.</param>
		/// <returns>Position in object-space.</returns>
		Mogre::Vector3 GetObjectSpacePosition(Mogre::Vector3 worldSpacePosition) {
			if(!disposed) {
				return GetManagedVector3(internalMesh->getObjectSpacePosition((Ogre::Vector3&)worldSpacePosition));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Get the world-space position from object-space position.
		/// </summary>
		/// <param name="objectSpacePosition">Position in object coords.</param>
		/// <returns>Position in world-space.</returns>
		Mogre::Vector3 GetWorldSpacePosition(Mogre::Vector3 objectSpacePosition) {
			if(!disposed) {
				return GetManagedVector3(internalMesh->getWorldSpacePosition((Ogre::Vector3&)objectSpacePosition));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets Mogre::MeshPtr.
		/// </summary>
		/// <returns>MeshPtr.</returns>
		property Mogre::MeshPtr^ MeshPtr {
			Mogre::MeshPtr^ get() {
				if(!disposed) {
					return (Mogre::MeshPtr^)internalMesh->getMesh();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets sub mesh.
		/// </summary>
		/// <returns>Sub mesh.</returns>
		property Mogre::SubMesh^ SubMesh {
			Mogre::SubMesh^ get() {
				if(!disposed) {
					return (Mogre::SubMesh^)internalMesh->getSubMesh();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets entity.
		/// </summary>
		/// <returns>Entity.</returns>
		property Mogre::Entity^ Entity {
			Mogre::Entity^ get() {
				if(!disposed) {
					return (Mogre::Entity^)internalMesh->getEntity();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the mesh size.
		/// </summary>
		/// <returns>Mesh size.</returns>
		property MSize^ Size {
			MSize^ get() {
				if(!disposed) {
					return gcnew MHydrax::MSize(internalMesh->getSize());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}
		 
		/// <summary>
		/// Gets the vertex type.
		/// </summary>
		/// <returns>Mesh vertex type.</returns>
		property MVertexType VertexType {
			MVertexType get() {
				if(!disposed) {
					return (MVertexType)internalMesh->getVertexType();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the number of faces.
		/// </summary>
		/// <returns>Number of faces.</returns>
		property int NumFaces {
			int get() {
				if(!disposed) {
					return internalMesh->getNumFaces();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the number of vertices.
		/// </summary>
		/// <returns>Number of vertices.</returns>
		property int NumVertices {
			int get() {
				if(!disposed) {
					return internalMesh->getNumVertices();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets or sets the mesh material name.
		/// </summary>
		/// <param name="value">Material name.</param>
		/// <returns>Material name.</returns>
		property System::String^ MaterialName {
			System::String^ get() {
				if(!disposed) {
					return GetManagedString(internalMesh->getMaterialName());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(System::String^ value) {
				if(!disposed) {
					return internalMesh->setMaterialName(GetUnmanagedString(value));
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets hardware vertex buffer.
		/// </summary>
		/// <returns>HardwareVertexBufferSharedPtr.</returns>
		property Mogre::HardwareVertexBufferSharedPtr^ HardwareVertexBufferPtr {
			Mogre::HardwareVertexBufferSharedPtr^ get() {
				if(!disposed) {
					return (Mogre::HardwareVertexBufferSharedPtr^)internalMesh->getHardwareVertexBuffer();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets hardware index buffer.
		/// </summary>
		/// <returns>HardwareIndexBufferSharedPtr.</returns>
		property Mogre::HardwareIndexBufferSharedPtr^ HardwareIndexBufferPtr {
			Mogre::HardwareIndexBufferSharedPtr^ get() {
				if(!disposed) {
					return (Mogre::HardwareIndexBufferSharedPtr^)internalMesh->getHardwareIndexBuffer();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Gets the SceneNode where Hydrax mesh is attached.
		/// </summary>
		/// <returns>SceneNode.</returns>
		property Mogre::SceneNode^ SceneNode {
			 Mogre::SceneNode^ get() {
				if(!disposed) {
					return (Mogre::SceneNode^)internalMesh->getSceneNode();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Has _createGeometry() been called already?
		/// </summary>
		/// <returns>True, if _createGeometry() has been called already. Otherwise False.</returns>
		property bool IsCreated {
			bool get() {
				if(!disposed) {
					return internalMesh->isCreated();
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MMesh() { // implements IDisposable
			if(!disposed) {
				// dispose managed resources

				this->!MMesh();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MMesh() {
			// dispose unmanaged resources
			if(ownsNativeRef) {
				delete internalMesh;
			}

			disposed = true;
		}

	};
}
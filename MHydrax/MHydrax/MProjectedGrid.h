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

// MProjectedGrid.h

#pragma once

#include "MModule.h"
#include "MHydrax.h"
#include "MNoise.h"
#include "MMaterialManager.h"

namespace MHydrax {

	/// <summary>
	/// MHydrax projected grid module.
	/// </summary>
	public ref class MProjectedGrid : MModule, System::IDisposable {
	public:
		/// <summary>
		/// Struct wich contains MHydrax projected grid module options.
		/// </summary>
		ref struct MOptions {
		private:
			Hydrax::Module::ProjectedGrid::Options* internalOptions;

			// Track whether this instance owns, or does not own the reference to the native pointer.
			bool ownsNativeRef;

			// Track whether Dispose has been called.
			bool disposed;

		internal:
			/// <summary>
			/// Creates a new instance of MProjectedGrid.MOptions.
			/// </summary>
			///	<param name="nativeOptions">A native Hydrax::Module::ProjectedGrid::Options.</param>
			MOptions(Hydrax::Module::ProjectedGrid::Options* nativeOptions) {
				internalOptions = nativeOptions;
				ownsNativeRef = false;
			}

		public:
			/// <summary>
			/// Creates a new instance of MProjectedGrid.MOptions.
			/// </summary>
			MOptions() {
				internalOptions = new Hydrax::Module::ProjectedGrid::Options();
				ownsNativeRef = true;
			}

			/// <summary>
			/// Creates a new instance of MProjectedGrid.MOptions.
			/// </summary>
			/// <param name="complexity">Projected grid complexity.</param>
			MOptions(int complexity) {
				internalOptions = new Hydrax::Module::ProjectedGrid::Options(
					complexity);
				ownsNativeRef = true;
			}

			/// <summary>
			/// Creates a new instance of MProjectedGrid.MOptions.
			/// </summary>
			/// <param name="complexity">Projected grid complexity.</param>
			/// <param name="strength">Perlin noise strength.</param>
			/// <param name="elevation">Elevation.</param>
			/// <param name="smooth">Smooth vertex?</param>
			MOptions(int complexity,
				    float strength,
					float elevation,
					bool smooth) {
				internalOptions = new Hydrax::Module::ProjectedGrid::Options(
					complexity, strength, elevation, smooth);
				ownsNativeRef = true;
			}

			/// <summary>
			/// Creates a new instance of MProjectedGrid.MOptions.
			/// </summary>
			/// <param name="complexity">Projected grid complexity.</param>
			/// <param name="strength">Perlin noise strength.</param>
			/// <param name="elevation">Elevation.</param>
			/// <param name="smooth">Smooth vertex?</param>
			/// <param name="forceRecalculateGeometry">Force to recalculate the projected grid geometry each frame.</param>
			/// <param name="choppyWaves">Choppy waves enabled? Note: Only with Materialmanager::NM_VERTEX normal mode.</param>
			/// <param name="choppyStrength">Choppy waves strength.</param>
			MOptions(int complexity,
				    float strength,
					float elevation,
					bool smooth,
					bool forceRecalculateGeometry,
					bool choppyWaves,
					float choppyStrength) {
				internalOptions = new Hydrax::Module::ProjectedGrid::Options(
					complexity, strength, elevation, smooth, forceRecalculateGeometry, choppyWaves, choppyStrength);
				ownsNativeRef = true;
			}

			/// <summary>
			/// Conversion operator to type Hydrax::Module::ProjectedGrid::Options*.
			/// </summary>
			operator Hydrax::Module::ProjectedGrid::Options *() {
				if(!disposed) {
					return internalOptions;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

			/// <summary>
			/// Conversion operator to type Hydrax::Module::ProjectedGrid::Options.
			/// </summary>
			operator Hydrax::Module::ProjectedGrid::Options(){
				if(!disposed) {
					Hydrax::Module::ProjectedGrid::Options opt = Hydrax::Module::ProjectedGrid::Options(internalOptions->Complexity, internalOptions->Strength, internalOptions->Elevation, internalOptions->Smooth, internalOptions->ForceRecalculateGeometry, internalOptions->ChoppyWaves, internalOptions->ChoppyStrength);
					return opt;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

			/// <summary>
			/// Complexity.
			/// </summary>
			property int Complexity {
				int get() {
					if(!disposed) {
						return internalOptions->Complexity;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(int value) {
					if(!disposed) {
						internalOptions->Complexity = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Strength.
			/// </summary>
			property float Strength {
				float get() {
					if(!disposed) {
						return internalOptions->Strength;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->Strength = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Elevation.
			/// </summary>
			property float Elevation {
				float get() {
					if(!disposed) {
						return internalOptions->Elevation;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->Elevation = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Smooth.
			/// </summary>
			property bool Smooth {
				bool get() {
					if(!disposed) {
						return internalOptions->Smooth;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(bool value) {
					if(!disposed) {
						internalOptions->Smooth = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// ForceRecalculateGeometry.
			/// </summary>
			property bool ForceRecalculateGeometry {
				bool get() {
					if(!disposed) {
						return internalOptions->ForceRecalculateGeometry;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(bool value) {
					if(!disposed) {
						internalOptions->ForceRecalculateGeometry = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// ChoppyWaves.
			/// </summary>
			property bool ChoppyWaves {
				bool get() {
					if(!disposed) {
						return internalOptions->ChoppyWaves;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(bool value) {
					if(!disposed) {
						internalOptions->ChoppyWaves = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// ChoppyStrength.
			/// </summary>
			property float ChoppyStrength {
				float get() {
					if(!disposed) {
						return internalOptions->ChoppyStrength;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->ChoppyStrength = value;
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

	private:
		Hydrax::Module::ProjectedGrid* internalPg;

		MNoise^ mNoise;

		// Track whether this instance owns, or does not own the reference to the native pointer.
		bool ownsNativeRef;

		// Track whether Dispose has been called.
		bool disposed;

	internal:
		/// <summary>
		/// Creates a new instance of MProjectedGrid.
		/// </summary>
		///	<param name="nativeProjectedGrid">A native Hydrax::Module::ProjectedGrid.</param>
		MProjectedGrid(Hydrax::Module::ProjectedGrid* nativeProjectedGrid)
			: MModule() {
			internalPg = nativeProjectedGrid;
			ownsNativeRef = false;
		}

		virtual property bool OwnsNativeRef {
			bool get() override {
				return ownsNativeRef;
			}
			void set(bool value) override {
				ownsNativeRef = value;
				mNoise->OwnsNativeRef = value;
			}
		}

	public:
		/// <summary>
		/// Conversion operator to type Hydrax::Module::Module*.
		/// </summary>
		virtual operator Hydrax::Module::Module *() override {
			if(!disposed) {
				return (Hydrax::Module::Module*)internalPg;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Conversion operator to type Hydrax::Module::ProjectedGrid*.
		/// </summary>
		operator Hydrax::Module::ProjectedGrid *() {
			if(!disposed) {
				return internalPg;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets the MHydrax::MNoise.
		/// </summary>
		/// <returns>The current MHydrax::MNoise.</returns>
		virtual property MNoise^ Noise {
			MNoise^ get() override {
				if(!disposed) {
					return mNoise;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Creates a new instance of MProjectedGrid.
		/// </summary>
		/// <param name="h">Hydrax manager pointer.</param>
		///	<param name="n">Hydrax noise module.</param>
		///	<param name="bp">Noise base plane.</param>
		///	<param name="nm">Switch between MaterialManager::NM_VERTEX and Materialmanager::NM_RTT.</param>
		MProjectedGrid(MHydrax^ h,
					   MNoise^ n,
					   Mogre::Plane bp,
					   MMaterialManager::MNormalMode nm) : MModule()
		{
			internalPg = new Hydrax::Module::ProjectedGrid((Hydrax::Hydrax*)h, (Hydrax::Noise::Noise*)n, (Ogre::Plane&)bp, (Hydrax::MaterialManager::NormalMode)nm);
			mNoise = n;
			ownsNativeRef = true;
		}

		/// <summary>
		/// Creates a new instance of MProjectedGrid.
		/// </summary>
		/// <param name="h">Hydrax manager pointer.</param>
		///	<param name="n">Hydrax noise module.</param>
		///	<param name="bp">Noise base plane.</param>
		///	<param name="nm">Switch between MaterialManager::NM_VERTEX and Materialmanager::NM_RTT.</param>
		///	<param name="opt">Perlin options.</param>
		MProjectedGrid(MHydrax^ h,
					   MNoise^ n,
					   Mogre::Plane bp,
					   MMaterialManager::MNormalMode nm,
					   MProjectedGrid::MOptions^ opt)
		{
			Hydrax::Module::ProjectedGrid::Options nativeOptions = (Hydrax::Module::ProjectedGrid::Options)*opt;
			internalPg = new Hydrax::Module::ProjectedGrid((Hydrax::Hydrax*)h, (Hydrax::Noise::Noise*)n, (Ogre::Plane&)bp, (Hydrax::MaterialManager::NormalMode)nm, nativeOptions);
			mNoise = n;
			ownsNativeRef = true;
		}

		/// <summary>
		/// Create.
		/// </summary>
		virtual void Create() override {
			if(!disposed) {
				internalPg->create();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Remove.
		/// </summary>
		virtual void Remove() override {
			if(!disposed) {
				internalPg->remove();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Call it each frame.
		/// </summary>
		/// <param name="timeSinceLastFrame">Time since last frame(delta).</param>
		virtual void Update(float timeSinceLastFrame) override {
			// Leave out the check for object disposed since its called every frame.
			internalPg->update(timeSinceLastFrame);
		}

		/// <summary>
		/// Save config.
		/// </summary>
		/// <param name="data">String reference.</param>
		virtual void SaveCfg(System::String^ data) override {
			if(!disposed) {
				internalPg->saveCfg(GetUnmanagedString(data));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Load config.
		/// </summary>
		/// <param name="cfgFile">A Mogre::ConfigFile.</param>
		/// <returns>True if it is the correct module config.</returns>
		virtual bool LoadCfg(Mogre::ConfigFile^ cfgFile) override {
			if(!disposed) {
				return internalPg->loadCfg(cfgFile);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Has Create() already been called?
		/// </summary>
		/// <returns>True, if Create() has been called already. Otherwise False.</returns>
		virtual property bool IsCreated {
			bool get() override {
				return internalPg->isCreated();
			}
		}

		/// <summary>
		/// Gets the current height at a specified world-space point.
		/// </summary>
		/// <param name="position">X/Z World position.</param>
		/// <returns>The Height at the given position in y-World coordinates. Returns -1, if it's outside of the water.</returns>
		float GetHeight(Mogre::Vector2 position) {
			if(!disposed) {
				return internalPg->getHeigth((Ogre::Vector2&)position);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets the current options.
		/// </summary>
		/// <param name="value">Options.</param>
		/// <returns>Current options.</returns>
		property MProjectedGrid::MOptions^ Options {
			MProjectedGrid::MOptions^ get() {
				if(!disposed) {
					return gcnew MProjectedGrid::MOptions((Hydrax::Module::ProjectedGrid::Options*)&internalPg->getOptions());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(MProjectedGrid::MOptions^ value) {
				if(!disposed) {
					Hydrax::Module::ProjectedGrid::Options opt = (Hydrax::Module::ProjectedGrid::Options)*value;
					internalPg->setOptions(opt);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MProjectedGrid() { // implements IDisposable
			if(!disposed) {
				// dispose managed resources
				delete mNoise;

				this->!MProjectedGrid();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MProjectedGrid() {
			// dispose unmanaged resources
			if(ownsNativeRef) {
				delete internalPg;
			}

			disposed = true;
		}

	};
}

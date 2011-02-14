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

// MRadialGrid.h

#pragma once

#include "MModule.h"
#include "MHydrax.h"
#include "MNoise.h"
#include "MMaterialManager.h"

namespace MHydrax {

	/// <summary>
	/// MHydrax radial grid module
	/// </summary>
	public ref class MRadialGrid : MModule, System::IDisposable {
	public:
		/// <summary>
		/// Struct wich contains MHydrax radial grid module options.
		/// </summary>
		ref struct MOptions {
		private:
			Hydrax::Module::RadialGrid::Options* internalOptions;

			// Track whether this instance owns, or does not own the reference to the native pointer.
			bool ownsNativeRef;

			// Track whether Dispose has been called.
			bool disposed;

		internal:
			/// <summary>
			/// Creates a new instance of MRadialGrid.MOptions.
			/// </summary>
			///	<param name="nativeOptions">A native Hydrax::Module::RadialGrid::Options.</param>
			MOptions(Hydrax::Module::RadialGrid::Options* nativeOptions) {
				internalOptions = nativeOptions;
				ownsNativeRef = false;
			}

		public:
			/// <summary>
			/// Creates a new instance of MRadialGrid.MOptions.
			/// </summary>
			MOptions() {
				internalOptions = new Hydrax::Module::RadialGrid::Options();
				ownsNativeRef = true;
			}

			/// <summary>
			/// Creates a new instance of MRadialGrid.MOptions.
			/// </summary>
			/// <param name="steps">Number of steps per circle.</param>
			/// <param name="circles">Number of circles.</param>
			/// <param name="radius">Mesh radius.</param>
			MOptions(int steps,
					int circles,
				    float radius) {
				internalOptions = new Hydrax::Module::RadialGrid::Options(
					steps, circles, radius);
				ownsNativeRef = true;
			}

			/// <summary>
			/// Creates a new instance of MRadialGrid.MOptions.
			/// </summary>
			/// <param name="steps">Number of steps per circle.</param>
			/// <param name="circles">Number of circles.</param>
			/// <param name="radius">Mesh radius.</param>
			/// <param name="smooth">Smooth vertex?</param>
			/// <param name="choppyWaves">Choppy waves enabled? Note: Only with Materialmanager::NM_VERTEX normal mode.</param>
			/// <param name="choppyStrength">Choppy waves strength. Note: Only with Materialmanager::NM_VERTEX normal mode.</param>
			/// <param name="stepSizeCube">Step cube size.</param>
			/// <param name="stepSizeFive">Step five size.</param>
			/// <param name="stepSizeLin">Step lin size.</param>
			/// <param name="strength">Water strength.</param>
			MOptions(int steps,
					int circles,
				    float radius,
					bool smooth,
					bool choppyWaves,
					float choppyStrength,
					float stepSizeCube,
					float stepSizeFive,
					float stepSizeLin,
					float strength) {
				internalOptions = new Hydrax::Module::RadialGrid::Options(
					steps, circles, radius, smooth, choppyWaves, choppyStrength, stepSizeCube, stepSizeFive, stepSizeLin, strength);
				ownsNativeRef = true;
			}

			/// <summary>
			/// Conversion operator to type Hydrax::Module::RadialGrid::Options*.
			/// </summary>
			operator Hydrax::Module::RadialGrid::Options *() {
				if(!disposed) {
					return internalOptions;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

			/// <summary>
			/// Conversion operator to type Hydrax::Module::RadialGrid::Options.
			/// </summary>
			operator Hydrax::Module::RadialGrid::Options(){
				if(!disposed) {
					Hydrax::Module::RadialGrid::Options opt = Hydrax::Module::RadialGrid::Options(internalOptions->Steps, internalOptions->Circles, internalOptions->Radius, internalOptions->Smooth, internalOptions->ChoppyWaves, internalOptions->ChoppyStrength, internalOptions->StepSizeCube, internalOptions->StepSizeFive, internalOptions->StepSizeLin, internalOptions->Strength);
					return opt;
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}

			/// <summary>
			/// Steps.
			/// </summary>
			property int Steps {
				int get() {
					if(!disposed) {
						return internalOptions->Steps;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(int value) {
					if(!disposed) {
						internalOptions->Steps = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Circles.
			/// </summary>
			property int Circles {
				int get() {
					if(!disposed) {
						return internalOptions->Circles;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(int value) {
					if(!disposed) {
						internalOptions->Circles = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Radius.
			/// </summary>
			property float Radius {
				float get() {
					if(!disposed) {
						return internalOptions->Radius;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->Radius = value;
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
			/// StepSizeCube.
			/// </summary>
			property float StepSizeCube {
				float get() {
					if(!disposed) {
						return internalOptions->StepSizeCube;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->StepSizeCube = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// StepSizeFive.
			/// </summary>
			property float StepSizeFive {
				float get() {
					if(!disposed) {
						return internalOptions->StepSizeFive;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->StepSizeFive = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// StepSizeLin.
			/// </summary>
			property float StepSizeLin {
				float get() {
					if(!disposed) {
						return internalOptions->StepSizeLin;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
				void set(float value) {
					if(!disposed) {
						internalOptions->StepSizeLin = value;
					} else {
						throw gcnew System::ObjectDisposedException(this->ToString());
					}
				}
			}

			/// <summary>
			/// Water Strength.
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
		Hydrax::Module::RadialGrid* internalRg;

		MNoise^ mNoise;

		// Track whether this instance owns, or does not own the reference to the native pointer.
		bool ownsNativeRef;

		// Track whether Dispose has been called.
		bool disposed;

	internal:
		/// <summary>
		/// Creates a new instance of MRadialGrid.
		/// </summary>
		///	<param name="nativeRadialGrid">A native Hydrax::Module::RadialGrid.</param>
		MRadialGrid(Hydrax::Module::RadialGrid* nativeRadialGrid)
			: MModule() {
			internalRg = nativeRadialGrid;
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
				return (Hydrax::Module::Module*)internalRg;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Conversion operator to type Hydrax::Module::RadialGrid*.
		/// </summary>
		operator Hydrax::Module::RadialGrid *() {
			if(!disposed) {
				return internalRg;
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
		/// Creates a new instance of MRadialGrid.
		/// </summary>
		/// <param name="h">Hydrax manager pointer.</param>
		///	<param name="n">Hydrax noise module.</param>
		///	<param name="nm">Switch between MaterialManager::NM_VERTEX and Materialmanager::NM_RTT.</param>
		MRadialGrid(MHydrax^ h,
					   MNoise^ n,
					   MMaterialManager::MNormalMode nm)
		{
			internalRg = new Hydrax::Module::RadialGrid((Hydrax::Hydrax*)h, (Hydrax::Noise::Noise*)n, (Hydrax::MaterialManager::NormalMode)nm);
			mNoise = n;
			ownsNativeRef = true;
		}

		/// <summary>
		/// Creates a new instance of MRadialGrid.
		/// </summary>
		/// <param name="h">Hydrax manager pointer.</param>
		///	<param name="n">Hydrax noise module.</param>
		///	<param name="nm">Switch between MaterialManager::NM_VERTEX and Materialmanager::NM_RTT.</param>
		///	<param name="opt">Perlin options.</param>
		MRadialGrid(MHydrax^ h,
					   MNoise^ n,
					   MMaterialManager::MNormalMode nm,
					   MRadialGrid::MOptions^ opt)
		{
			Hydrax::Module::RadialGrid::Options nativeOptions = (Hydrax::Module::RadialGrid::Options)*opt;
			internalRg = new Hydrax::Module::RadialGrid((Hydrax::Hydrax*)h, (Hydrax::Noise::Noise*)n, (Hydrax::MaterialManager::NormalMode)nm, nativeOptions);
			mNoise = n;
			ownsNativeRef = true;
		}

		/// <summary>
		/// Create.
		/// </summary>
		virtual void Create() override {
			if(!disposed) {
				internalRg->create();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Remove.
		/// </summary>
		virtual void Remove() override {
			if(!disposed) {
				internalRg->remove();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Call it each frame.
		/// </summary>
		/// <param name="timeSinceLastFrame">Time since last frame(delta).</param>
		virtual void Update(float timeSinceLastFrame) override {
			if(!disposed) {
				internalRg->update(timeSinceLastFrame);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Save config.
		/// </summary>
		/// <param name="data">String reference.</param>
		virtual void SaveCfg(System::String^ data) override {
			if(!disposed) {
				internalRg->saveCfg(GetUnmanagedString(data));
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
				return internalRg->loadCfg(cfgFile);
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
				return internalRg->isCreated();
			}
		}

		/// <summary>
		/// Gets the current height at a specified world-space point.
		/// </summary>
		/// <param name="position">X/Z World position.</param>
		/// <returns>The Height at the given position in y-World coordinates. Returns -1, if it's outside of the water.</returns>
		float GetHeight(Mogre::Vector2 position) {
			if(!disposed) {
				return internalRg->getHeigth((Ogre::Vector2&)position);
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Gets the current options.
		/// </summary>
		/// <param name="value">Options.</param>
		/// <returns>Current options.</returns>
		property MRadialGrid::MOptions^ Options {
			MRadialGrid::MOptions^ get() {
				if(!disposed) {
					return gcnew MRadialGrid::MOptions((Hydrax::Module::RadialGrid::Options*)&internalRg->getOptions());
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
			void set(MRadialGrid::MOptions^ value) {
				if(!disposed) {
					Hydrax::Module::RadialGrid::Options opt = (Hydrax::Module::RadialGrid::Options)*value;
					internalRg->setOptions(opt);
				} else {
					throw gcnew System::ObjectDisposedException(this->ToString());
				}
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MRadialGrid() { // implements IDisposable
			if(!disposed) {
				// dispose managed resources
				delete mNoise;

				this->!MRadialGrid();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MRadialGrid() {
			// dispose unmanaged resources
			if(ownsNativeRef) {
				delete internalRg;
			}

			disposed = true;
		}

	};
}

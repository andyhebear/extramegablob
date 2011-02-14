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

// MModule.h

#pragma once

namespace MHydrax {

	/// <summary>
	/// Base module class, override it to create different ways of creating a water noise.
	/// </summary>
	public ref class MModule abstract : System::IDisposable {
	private:

	internal:
		virtual property bool OwnsNativeRef {
			bool get() = 0;
			void set(bool value) = 0;
		}

	public:
		/// <summary>
		/// Conversion operator to type Hydrax::Module::Module*.
		/// </summary>
		virtual operator Hydrax::Module::Module *() = 0;

		/// <summary>
		/// Gets the MHydrax::MNoise.
		/// </summary>
		/// <returns>The current MHydrax::MNoise.</returns>
		virtual property MNoise^ Noise {
			MNoise^ get() = 0;
		}

		/// <summary>
		/// Create.
		/// </summary>
		virtual void Create() = 0;

		/// <summary>
		/// Remove.
		/// </summary>
		virtual void Remove() = 0;

		/// <summary>
		/// Call it each frame.
		/// </summary>
		/// <param name="timeSinceLastFrame">Time since last frame(delta).</param>
		virtual void Update(float timeSinceLastFrame) = 0;

		/// <summary>
		/// Save config.
		/// </summary>
		/// <param name="data">String reference.</param>
		virtual void SaveCfg(System::String^ data) = 0;

		/// <summary>
		/// Load config.
		/// </summary>
		/// <param name="cfgFile">A Mogre::ConfigFile.</param>
		/// <returns>True if it is the correct module config.</returns>
		virtual bool LoadCfg(Mogre::ConfigFile^ cfgFile) = 0;

		/// <summary>
		/// Has Create() already been called?
		/// </summary>
		/// <returns>True, if Create() has been called already. Otherwise False.</returns>
		virtual property bool IsCreated {
			bool get() = 0;
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MModule() { // implements IDisposable
			// dispose managed resources

			this->!MModule();
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MModule() {
			// dispose unmanaged resources

		}

	};
}
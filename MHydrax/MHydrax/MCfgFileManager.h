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

// MCfgFileManager.h

#pragma once

namespace MHydrax {

	/// <summary>
	/// Config file manager.
	/// Class to load/save all Hydrax options from/to a config file.
	/// </summary>
	public ref class MCfgFileManager : System::IDisposable {
	private:
		Hydrax::CfgFileManager* internalCfgFileManager;

		// Track whether this instance owns, or does not own the reference to the native pointer.
		bool ownsNativeRef;

		// Track whether Dispose has been called.
		bool disposed;

	internal:
		/// <summary>
		/// Creates a new instance of MCfgFileManager.
		/// </summary>
		///	<param name="nativeCfgFileManager">A native Hydrax::CfgFileManager.</param>
		MCfgFileManager(Hydrax::CfgFileManager* nativeCfgFileManager) {
			internalCfgFileManager = nativeCfgFileManager;
			ownsNativeRef = false;
		}

	public:
		/// <summary>
		/// Conversion operator to type Hydrax::CfgFileManager*.
		/// </summary>
		operator Hydrax::CfgFileManager *() {
			if(!disposed) {
				return internalCfgFileManager;
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Load hydrax cfg file.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <returns>True if successful. False if an error has ocurred (Check the log file in this case).</returns>
		bool Load(System::String^ fileName) {
			if(!disposed) {
				return internalCfgFileManager->load(GetUnmanagedString(fileName));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Save current hydrax config to a file.
		/// </summary>
		/// <param name="fileName">Destination file name.</param>
		/// <param name="path">File path.</param>
		/// <returns>True if successful. False if an error has ocurred (Check the log file in this case).</returns>
		bool Save(System::String^ fileName, System::String^ path) {
			if(!disposed) {
				return internalCfgFileManager->save(GetUnmanagedString(fileName), GetUnmanagedString(path));
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

		/// <summary>
		/// Save current hydrax config to a file.
		/// </summary>
		/// <param name="fileName">Destination file name.</param>
		/// <returns>True if successful. False if an error has ocurred (Check the log file in this case).</returns>
		bool Save(System::String^ fileName) {
			return Save(fileName, "");
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MCfgFileManager() { // implements IDisposable
			if(!disposed) {
				// dispose managed resources

				this->!MCfgFileManager();
			} else {
				throw gcnew System::ObjectDisposedException(this->ToString());
			}
		}

	protected:
		/// <summary>
		/// Finalizer.
		/// </summary>
		!MCfgFileManager() {
			// dispose unmanaged resources
			if(ownsNativeRef) {
				delete internalCfgFileManager;
			}

			disposed = true;
		}

	};
}

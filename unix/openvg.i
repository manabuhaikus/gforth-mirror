// this file is in the public domain
%module soil
%insert("include")
%{
#include "openvg.h"
%}

%apply SWIGTYPE * { unsigned char const *const };

%include <openvg.h>

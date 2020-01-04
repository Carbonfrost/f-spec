FRAMEWORK ?= netcoreapp3.0
PREFIX = /usr/local

.PHONY: dotnet/install dotnet/generate

## Install dotnet outputs
dotnet/install: -install-dotnet-fspec

## Test dotnet
dotnet/test: dotnet/build
	dotnet dotnet/src/fspec/bin/$(CONFIGURATION)/$(FRAMEWORK)/fspec.dll --self-test

## Generate generated code
dotnet/generate:
	srgen -c Carbonfrost.Commons.Spec.Resources.SR \
		-r Carbonfrost.Commons.Spec.Automation.SR \
		dotnet/src/Carbonfrost.Commons.Spec/Automation/SR.properties \
		--resx
	srgen -c Carbonfrost.CFSpec.Resources.SR \
		-r Carbonfrost.CFSpec.Resources.SR \
		dotnet/src/fspec/Automation/SR.properties \
		--resx

-install-dotnet-%: -check-env-CONFIGURATION -check-env-FRAMEWORK -check-env-PREFIX
	@ rm -rf $(PREFIX)/opt/$*/
	@ mkdir -p $(PREFIX)/opt/$*/
	@ sed 's/APP_NAME/$*/g' ./dotnet/src/shim.template.sh > $(PREFIX)/bin/$*
	@ chmod +x $(PREFIX)/bin/$*
	@ cp -r ./dotnet/src/$*/bin/$(CONFIGURATION)/$(FRAMEWORK)/ $(PREFIX)/opt/$*/
	@ cp ./dotnet/src/runtimeconfig.template.json $(PREFIX)/opt/$*/$*.runtimeconfig.json

include eng/.mk/*.mk

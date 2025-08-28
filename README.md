[28/08, 11:39 am] Biswajit: 2025-08-28T05:54:24.1735235Z ##[section]Starting: npm publish
2025-08-28T05:54:24.1743028Z ==============================================================================
2025-08-28T05:54:24.1743537Z Task         : npm
2025-08-28T05:54:24.1743824Z Description  : Install and publish npm packages, or run an npm command. Supports npmjs.com and authenticated registries like Azure Artifacts.
2025-08-28T05:54:24.1744242Z Version      : 1.260.0
2025-08-28T05:54:24.1744519Z Author       : Microsoft Corporation
2025-08-28T05:54:24.1744794Z Help         : https://docs.microsoft.com/azure/devops/pipelines/tasks/package/npm
2025-08-28T05:54:24.1745081Z ==============================================================================
2025-08-28T05:54:25.5359042Z [command]/opt/hostedtoolcache/node/16.20.2/x64/bin/npm config list
2025-08-28T05:54:25.8409266Z ; "user" config from /home/vsts/work/1/npm/3137901.npmrc
2025-08-28T05:54:25.8421533Z 
2025-08-28T05:54:25.8423563Z //itsals.pkgs.visualstudio.com/_packaging/407b7af8-3da7-7837-973d-0e8256aaad5d/npm/registry/:_password = (protected) 
2025-08-28T05:54:25.8425599Z //itsals.pkgs.visualstudio.com/_packaging/407b7af8-3da7-7837-973d-0e8256aaad5d/npm/registry/:email = "VssEmail" 
2025-08-28T05:54:25.8426222Z //itsals.pkgs.visualstudio.com/_packaging/407b7af8-3da7-7837-973d-0e8256aaad5d/npm/registry/:username = "VssToken" 
2025-08-28T05:54:25.8426828Z ; registry = "https://itsals.pkgs.visualstudio.com/_packaging/407b7af8-3da7-7837-973d-0e8256aaad5d/npm/registry/" ; overridden by project
2025-08-28T05:54:25.8427188Z 
2025-08-28T05:54:25.8427564Z ; "project" config from /home/vsts/work/1/s/.npmrc
2025-08-28T05:54:25.8427834Z 
2025-08-28T05:54:25.8428172Z always-auth = true 
2025-08-28T05:54:25.8428603Z registry = "https://itsals.pkgs.visualstudio.com/_packaging/ASInternal/npm/registry/" 
2025-08-28T05:54:25.8428921Z 
2025-08-28T05:54:25.8429297Z ; "env" config from environment
2025-08-28T05:54:25.8429545Z 
2025-08-28T05:54:25.8429910Z userconfig = "/home/vsts/work/1/npm/3137901.npmrc" 
2025-08-28T05:54:25.8430217Z 
2025-08-28T05:54:25.8430592Z ; node bin location = /opt/hostedtoolcache/node/16.20.2/x64/bin/node
2025-08-28T05:54:25.8431825Z ; node version = v16.20.2
2025-08-28T05:54:25.8432197Z ; npm local prefix = /home/vsts/work/1/s
2025-08-28T05:54:25.8432527Z ; npm version = 8.19.4
2025-08-28T05:54:25.8432867Z ; cwd = /home/vsts/work/1/s
2025-08-28T05:54:25.8433187Z ; HOME = /home/vsts
2025-08-28T05:54:25.8433541Z ; Run `npm config ls -l` to show all defaults.
2025-08-28T05:54:25.8436381Z [command]/opt/hostedtoolcache/node/16.20.2/x64/bin/npm publish
2025-08-28T05:54:27.7550276Z 
2025-08-28T05:54:27.7556256Z > @alaskaair/cmscrew-splitio-ng14@1.0.4 publish
2025-08-28T05:54:27.7557517Z > npm publish dist/alaskaair-cmscrew-splitio
2025-08-28T05:54:27.7558929Z 
2025-08-28T05:54:27.7571184Z npm notice 
2025-08-28T05:54:27.7571897Z npm notice 📦  @alaskaair/cmscrew-splitio-ng14@1.0.4
2025-08-28T05:54:27.7592565Z npm notice === Tarball Contents === 
2025-08-28T05:54:27.7593005Z npm notice 5.1kB  README.md                                                             
2025-08-28T05:54:27.7951265Z npm notice 2.6kB  package.json                                                          
2025-08-28T05:54:27.7951856Z npm notice 181B   projects/alaskaair/cmscrew-splitio/ng-package.json                    
2025-08-28T05:54:27.7952274Z npm notice 641B   projects/alaskaair/cmscrew-splitio/package.json                       
2025-08-28T05:54:27.7952685Z npm notice 563B   projects/alaskaair/cmscrew-splitio/src/lib/cmscrew-splitio.module.ts  
2025-08-28T05:54:27.7953108Z npm notice 5.0kB  projects/alaskaair/cmscrew-splitio/src/lib/split.service.ts           
2025-08-28T05:54:27.7953511Z npm notice 143B   projects/alaskaair/cmscrew-splitio/src/public-api.ts                  
2025-08-28T05:54:27.7953946Z npm notice 25.6kB projects/coverage/alaskaair-cmscrew-splitio/html/split.service.ts.html
2025-08-28T05:54:27.7954378Z npm notice === Tarball Details === 
2025-08-28T05:54:27.7954739Z npm notice name:          @alaskaair/cmscrew-splitio-ng14          
2025-08-28T05:54:27.7955550Z npm notice version:       1.0.4                                    
2025-08-28T05:54:27.7955902Z npm notice filename:      @alaskaair/cmscrew-splitio-ng14-1.0.4.tgz
2025-08-28T05:54:27.7956252Z npm notice package size:  7.5 kB                                   
2025-08-28T05:54:27.7956575Z npm notice unpacked size: 39.8 kB                                  
2025-08-28T05:54:27.7956929Z npm notice shasum:        b8eefe3b8875ceb1eac9006026dc62678748077d 
2025-08-28T05:54:27.7957314Z npm notice integrity:     sha512-Bg0a5Ug1/r0v+[...]JAkwgC/9FLZJw== 
2025-08-28T05:54:27.7957646Z npm notice total files:   8                                        
2025-08-28T05:54:27.7957904Z npm notice 
2025-08-28T05:54:27.7958240Z npm notice Publishing to https://itsals.pkgs.visualstudio.com/_packaging/npm_***/npm/registry/
2025-08-28T05:54:27.7958601Z npm ERR! code 128
2025-08-28T05:54:27.7958904Z npm ERR! An unknown git error occurred
2025-08-28T05:54:27.7959349Z npm ERR! command git --no-replace-objects ls-remote ssh://git@github.com/dist/alaskaair-cmscrew-splitio.git
2025-08-28T05:54:27.7959782Z npm ERR! git@github.com: Permission denied (publickey).
2025-08-28T05:54:27.7960125Z npm ERR! fatal: Could not read from remote repository.
2025-08-28T05:54:27.7960408Z npm ERR! 
2025-08-28T05:54:27.7966936Z npm ERR! Please make sure you have the correct access rights
2025-08-28T05:54:27.7967537Z npm ERR! and the repository exists.
2025-08-28T05:54:27.7967805Z 
2025-08-28T05:54:27.7968169Z npm ERR! A complete log of this run can be found in:
2025-08-28T05:54:27.7968637Z npm ERR!     /home/vsts/.npm/_logs/2025-08-28T05_54_27_231Z-debug-0.log
2025-08-28T05:54:27.7969030Z npm ERR! code 128
2025-08-28T05:54:27.7969403Z npm ERR! path /home/vsts/work/1/s
2025-08-28T05:54:27.7969841Z npm ERR! command failed
2025-08-28T05:54:27.7970336Z npm ERR! command sh -c -- npm publish dist/alaskaair-cmscrew-splitio
2025-08-28T05:54:27.7970870Z 
2025-08-28T05:54:27.7971374Z npm ERR! A complete log of this run can be found in:
2025-08-28T05:54:27.7971870Z npm ERR!     /home/vsts/.npm/_logs/2025-08-28T05_54_26_109Z-debug-0.log
2025-08-28T05:54:27.8000123Z ##[warning]Couldn't find a debug log in the cache or working directory
2025-08-28T05:54:27.8006730Z ##[error]Error: Npm failed with return code: 128
2025-08-28T05:54:27.8013414Z ##[section]Finishing: npm publish
[28/08, 11:40 am] Biswajit: npm notice 25.6kB projects/coverage/alaskaair-cmscrew-splitio/html/split.service.ts.html
npm notice === Tarball Details === 
npm notice name:          @alaskaair/cmscrew-splitio-ng14          
npm notice version:       1.0.4                                    
npm notice filename:      @alaskaair/cmscrew-splitio-ng14-1.0.4.tgz
npm notice package size:  7.5 kB                                   
npm notice unpacked size: 39.8 kB                                  
npm notice shasum:        b8eefe3b8875ceb1eac9006026dc62678748077d 
npm notice integrity:     sha512-Bg0a5Ug1/r0v+[...]JAkwgC/9FLZJw== 
npm notice total files:   8                                        
npm notice 
npm notice Publishing to https://itsals.pkgs.visualstudio.com/_packaging/npm_***/npm/registry/
npm ERR! code 128
npm ERR! An unknown git error occurred
npm ERR! command git --no-replace-objects ls-remote ssh://git@github.com/dist/alaskaair-cmscrew-splitio.git
npm ERR! git@github.com: Permission denied (publickey).
npm ERR! fatal: Could not read from remote repository.
npm ERR! 
npm ERR! Please make sure you have the correct access rights
npm ERR! and the repository exists.

npm ERR! A complete log of this run can be found in:
npm ERR!     /home/vsts/.npm/_logs/2025-08-28T05_54_27_231Z-debug-0.log
npm ERR! code 128
npm ERR! path /home/vsts/work/1/s
npm ERR! command failed
npm ERR! command sh -c -- npm publish dist/alaskaair-cmscrew-splitio

npm ERR! A complete log of this run can be found in:
npm ERR!     /home/vsts/.npm/_logs/2025-08-28T05_54_26_109Z-debug-0.log
##[warning]Couldn't find a debug log in the cache or working directory
##[error]Error: Npm failed with return code: 128
Finishing: npm publish
[28/08, 11:42 am] Biswajit: parameters:
  publishFeed: 'ASInternal'
  nodeToolVersion: '16.x'

jobs:
- job: BuildAndPublish
  displayName: 'Build and publish npm'
  pool:
    vmImage: ubuntu-latest
  steps:
  - task: NodeTool@0
    displayName: 'Install Node.js'
    inputs:
      versionSpec: ${{ parameters.nodeToolVersion }}
    
  - task: Npm@1
    displayName: npm ci
    inputs:
      command: 'ci'

  - task: Npm@1
    displayName: 'npm build'
    inputs:
      command: 'custom'
      customCommand: 'run-script build'

  - task: Npm@1
    displayName: 'npm publish'
    inputs:
      command: publish
      publishRegistry: useFeed
      publishFeed: ${{ parameters.publishFeed }}
                {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotEmpty(data); // WireMock response
        }

        [Fact]
        public async Task Products_CRUD_Test_WithSQLite_AndWireMock()
        {
            var client = _factory.CreateClient();

            // GET
            var getResponse = await client.GetAsync("/api/products");
            getResponse.EnsureSuccessStatusCode();
            var getContent = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains("Test Product", getContent);

            // POST
            var newProduct = new Product { Name = "New Product", Price = 200 };
            var json = new StringContent(JsonSerializer.Serialize(newProduct), Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync("/api/products", json);
            postResponse.EnsureSuccessStatusCode();
            var postContent = await postResponse.Content.ReadAsStringAsync();
            Assert.Contains("Sunny", postContent); // WireMock
            Assert.Contains("New Product", postContent);

            // PUT
            var putProduct = new Product { Id = 1, Name = "Updated Product", Price = 150 };
            var putJson = new StringContent(JsonSerializer.Serialize(putProduct), Encoding.UTF8, "application/json");
            var putResponse = await client.PutAsync("/api/products/1", putJson);
            Assert.Equal(System.Net.HttpStatusCode.NoContent, putResponse.StatusCode);

            // Verify update
            var verifyResponse = await client.GetAsync("/api/products");
            verifyResponse.EnsureSuccessStatusCode();
            var verifyContent = await verifyResponse.Content.ReadAsStringAsync();
            Assert.Contains("Updated Product", verifyContent);

            // DELETE
            var deleteResponse = await client.DeleteAsync("/api/products/1");
            Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Verify deletion
            var finalGetResponse = await client.GetAsync("/api/products/1");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, finalGetResponse.StatusCode);
        }
    }
}

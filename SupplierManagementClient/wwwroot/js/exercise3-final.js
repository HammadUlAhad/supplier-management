/**
 * Exercise 3: Fresh Implementation - No Cache Issues
 * Production-ready API client for synchronous vs asynchronous demonstration
 */

console.log('🚀 Exercise 3 - Fresh Implementation Loaded!');

document.addEventListener('DOMContentLoaded', function() {
    console.log('✅ DOM Ready - Initializing Exercise 3');
    
    // Load configuration
    let config = null;
    let authToken = null;
    
    try {
        const configElement = document.getElementById('app-config');
        if (!configElement) {
            throw new Error('Configuration not found');
        }
        
        config = JSON.parse(configElement.textContent);
        console.log('✅ Configuration loaded successfully');
        console.log('🔧 API Base URL:', config.apiSettings.baseUrl);
        console.log('🔧 Auth Endpoint:', config.apiSettings.authEndpoint);
        
    } catch (error) {
        console.error('❌ Configuration error:', error);
        alert('Configuration Error: ' + error.message);
        return;
    }
    
    // Check for existing authentication
    const existingToken = sessionStorage.getItem('jwt_token');
    const existingExpiry = sessionStorage.getItem('jwt_expiry');
    
    if (existingToken && existingExpiry && new Date(existingExpiry) > new Date()) {
        authToken = existingToken;
        console.log('🔐 Found existing valid token');
        updateAuthStatus('success', '✅ Already authenticated');
        enableApiButtons();
    }
    
    // Get UI elements
    const btnAuth = document.getElementById('btnAuthenticate');
    const btnSync = document.getElementById('btnGetSuppliers');
    const btnAsync = document.getElementById('btnGetOverlaps');
    
    console.log('🔍 UI Elements found:');
    console.log('   Auth button:', !!btnAuth);
    console.log('   Sync button:', !!btnSync);
    console.log('   Async button:', !!btnAsync);
    
    // Bind authentication button with immediate effect
    if (btnAuth) {
        // Remove all existing event listeners by cloning
        const newBtn = btnAuth.cloneNode(true);
        btnAuth.parentNode.replaceChild(newBtn, btnAuth);
        
        // Add fresh event listener
        newBtn.onclick = async function(e) {
            e.preventDefault();
            console.log('🎯 FRESH AUTH CLICK DETECTED!');
            await handleAuthentication();
        };
        
        console.log('✅ Fresh authentication handler attached');
    }
    
    // Bind sync button
    if (btnSync) {
        btnSync.onclick = function(e) {
            e.preventDefault();
            console.log('🔄 SYNC CALL TRIGGERED!');
            handleSyncCall();
        };
        console.log('✅ Sync handler attached');
    }
    
    // Bind async button
    if (btnAsync) {
        btnAsync.onclick = function(e) {
            e.preventDefault();
            console.log('⚡ ASYNC CALL TRIGGERED!');
            handleAsyncCall();
        };
        console.log('✅ Async handler attached');
    }
    
    // Authentication function
    async function handleAuthentication() {
        console.log('🔑 Starting authentication...');
        
        const startTime = performance.now();
        const btn = document.getElementById('btnAuthenticate');
        
        if (!btn) {
            console.error('❌ Auth button not found during execution');
            return;
        }
        
        // Update UI
        btn.disabled = true;
        btn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Authenticating...';
        updateAuthStatus('info', '🔄 Authenticating...');
        
        try {
            const url = `${config.apiSettings.baseUrl}${config.apiSettings.authEndpoint}`;
            console.log('📡 Making auth request to:', url);
            
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({
                    username: config.credentials.username,
                    password: config.credentials.password
                })
            });
            
            console.log('📡 Response status:', response.status);
            
            if (!response.ok) {
                const errorText = await response.text();
                console.error('❌ Auth failed:', errorText);
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }
            
            const data = await response.json();
            console.log('📦 Auth response:', data);
            
            if (data.success && data.token && data.token.token) {
                authToken = data.token.token;
                const authTime = performance.now() - startTime;
                
                // Store token
                sessionStorage.setItem('jwt_token', authToken);
                sessionStorage.setItem('jwt_expiry', data.token.expiration || new Date(Date.now() + 24*60*60*1000).toISOString());
                
                console.log(`✅ Authentication successful in ${authTime.toFixed(2)}ms`);
                console.log('🔐 Token:', authToken.substring(0, 50) + '...');
                
                updateAuthStatus('success', `✅ Authenticated (${authTime.toFixed(0)}ms)`);
                enableApiButtons();
                
            } else {
                throw new Error('Invalid response format');
            }
            
        } catch (error) {
            console.error('❌ Authentication error:', error);
            updateAuthStatus('danger', `❌ Error: ${error.message}`);
        } finally {
            // Reset button
            btn.disabled = false;
            btn.innerHTML = '<i class="fas fa-key me-2"></i>Get JWT Token';
        }
    }
    
    // Synchronous API call
    function handleSyncCall() {
        if (!authToken) {
            alert('Please authenticate first!');
            return;
        }
        
        console.log('🔄 Starting SYNCHRONOUS call...');
        const startTime = performance.now();
        
        updateSyncStatus('warning', '⚠️ BLOCKING: UI will freeze');
        disableAllButtons();
        showLoading('Synchronous call in progress...');
        
        try {
            const xhr = new XMLHttpRequest();
            const url = `${config.apiSettings.baseUrl}${config.apiSettings.suppliersEndpoint}`;
            
            console.log('📡 Sync request to:', url);
            
            // Synchronous request (blocks UI)
            xhr.open('GET', url, false);
            xhr.setRequestHeader('Authorization', `Bearer ${authToken}`);
            xhr.setRequestHeader('Accept', 'application/json');
            
            xhr.send(); // This blocks the UI
            
            const syncTime = performance.now() - startTime;
            
            if (xhr.status === 200) {
                const data = JSON.parse(xhr.responseText);
                console.log(`✅ Sync call completed in ${syncTime.toFixed(2)}ms`);
                console.log('📦 Sync data:', data);
                
                displayResults('Suppliers (Synchronous)', data, 'sync', syncTime);
                updateSyncStatus('success', `✅ Completed (${syncTime.toFixed(0)}ms)`);
                
            } else {
                throw new Error(`HTTP ${xhr.status}: ${xhr.statusText}`);
            }
            
        } catch (error) {
            console.error('❌ Sync call failed:', error);
            updateSyncStatus('danger', `❌ Error: ${error.message}`);
        } finally {
            hideLoading();
            enableAllButtons();
        }
    }
    
    // Asynchronous API call
    async function handleAsyncCall() {
        if (!authToken) {
            alert('Please authenticate first!');
            return;
        }
        
        console.log('⚡ Starting ASYNCHRONOUS call...');
        const startTime = performance.now();
        
        updateAsyncStatus('info', '🔄 NON-BLOCKING: UI responsive');
        showLoading('Asynchronous call in progress...');
        
        try {
            const url = `${config.apiSettings.baseUrl}${config.apiSettings.overlapsEndpoint}`;
            console.log('📡 Async request to:', url);
            
            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${authToken}`,
                    'Accept': 'application/json'
                }
            });
            
            const asyncTime = performance.now() - startTime;
            
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }
            
            const data = await response.json();
            console.log(`✅ Async call completed in ${asyncTime.toFixed(2)}ms`);
            console.log('📦 Async data:', data);
            
            displayResults('Overlapping Rates (Asynchronous)', data, 'async', asyncTime);
            updateAsyncStatus('success', `✅ Completed (${asyncTime.toFixed(0)}ms)`);
            
        } catch (error) {
            console.error('❌ Async call failed:', error);
            updateAsyncStatus('danger', `❌ Error: ${error.message}`);
        } finally {
            hideLoading();
        }
    }
    
    // Helper functions
    function updateAuthStatus(type, message) {
        const element = document.getElementById('authStatus');
        if (element) {
            element.className = `alert alert-${type} mb-0`;
            element.innerHTML = `<i class="fas fa-info-circle me-2"></i>${message}`;
        }
    }
    
    function updateSyncStatus(type, message) {
        const element = document.getElementById('syncStatus');
        if (element) {
            element.innerHTML = `<small class="text-${type}">${message}</small>`;
        }
    }
    
    function updateAsyncStatus(type, message) {
        const element = document.getElementById('asyncStatus');
        if (element) {
            element.innerHTML = `<small class="text-${type}">${message}</small>`;
        }
    }
    
    function enableApiButtons() {
        const syncBtn = document.getElementById('btnGetSuppliers');
        const asyncBtn = document.getElementById('btnGetOverlaps');
        
        if (syncBtn) syncBtn.disabled = false;
        if (asyncBtn) asyncBtn.disabled = false;
    }
    
    function disableAllButtons() {
        const buttons = ['btnAuthenticate', 'btnGetSuppliers', 'btnGetOverlaps'];
        buttons.forEach(id => {
            const btn = document.getElementById(id);
            if (btn) btn.disabled = true;
        });
    }
    
    function enableAllButtons() {
        const authBtn = document.getElementById('btnAuthenticate');
        if (authBtn) authBtn.disabled = false;
        
        if (authToken) {
            enableApiButtons();
        }
    }
    
    function showLoading(message) {
        const section = document.getElementById('loadingSection');
        const messageEl = document.getElementById('loadingMessage');
        
        if (section) section.style.display = 'block';
        if (messageEl) messageEl.textContent = message;
    }
    
    function hideLoading() {
        const section = document.getElementById('loadingSection');
        if (section) section.style.display = 'none';
    }
    
    function displayResults(title, data, type, time) {
        const container = document.getElementById('apiResults');
        if (!container) return;
        
        const timestamp = new Date().toLocaleTimeString();
        const recordCount = getRecordCount(data);
        
        container.innerHTML = `
            <div class="mb-3">
                <h5 class="text-primary">
                    ${type === 'sync' ? '🔄' : '⚡'} ${title}
                    <span class="badge ${type === 'sync' ? 'bg-warning' : 'bg-primary'} ms-2">
                        ${type === 'sync' ? 'SYNCHRONOUS' : 'ASYNCHRONOUS'}
                    </span>
                </h5>
                <div class="row">
                    <div class="col-md-6">
                        <small class="text-muted">
                            <i class="fas fa-clock me-1"></i>Time: ${time.toFixed(2)}ms<br>
                            <i class="fas fa-database me-1"></i>Records: ${recordCount}<br>
                            <i class="fas fa-calendar me-1"></i>Timestamp: ${timestamp}
                        </small>
                    </div>
                    <div class="col-md-6">
                        <small class="text-muted">
                            <i class="fas fa-code me-1"></i>Method: ${type === 'sync' ? 'XMLHttpRequest (Blocking)' : 'Fetch API (Non-blocking)'}<br>
                            <i class="fas fa-chart-line me-1"></i>UI Behavior: ${type === 'sync' ? 'Frozen during call' : 'Responsive during call'}
                        </small>
                    </div>
                </div>
            </div>
            
            <div class="card">
                <div class="card-header">
                    <h6 class="mb-0">
                        <i class="fas fa-database me-2"></i>API Response Data
                    </h6>
                </div>
                <div class="card-body">
                    <pre class="bg-dark text-light p-3 rounded" style="max-height: 400px; overflow-y: auto;"><code>${JSON.stringify(data, null, 2)}</code></pre>
                </div>
            </div>
        `;
        
        // Update metrics badge
        const badge = document.getElementById('responseMetrics');
        if (badge) {
            badge.textContent = `${recordCount} records`;
            badge.style.display = 'inline';
        }
    }
    
    function getRecordCount(data) {
        if (data.suppliers) return data.suppliers.length;
        if (data.overlappingRates) return data.overlappingRates.length;
        if (Array.isArray(data)) return data.length;
        return 1;
    }
    
    // Manual testing functions
    window.testAuth = function() {
        console.log('🧪 Manual auth test');
        handleAuthentication();
    };
    
    window.testSync = function() {
        console.log('🧪 Manual sync test');
        handleSyncCall();
    };
    
    window.testAsync = function() {
        console.log('🧪 Manual async test');
        handleAsyncCall();
    };
    
    console.log('🔧 Exercise 3 initialization complete!');
    console.log('🧪 Manual test functions available: testAuth(), testSync(), testAsync()');
});

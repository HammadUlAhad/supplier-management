// Exercise 3: Client-side API consumption with synchronous and asynchronous calls
// API Configuration
const API_CONFIG = {
    baseUrl: 'http://localhost:5114',
    endpoints: {
        auth: '/api/v1/auth/token',
        suppliers: '/api/v1/suppliers',
        overlaps: '/api/v1/suppliers/overlaps'
    },
    credentials: {
        username: 'admin',
        password: 'password123'
    }
};

// Global variables
let jwtToken = null;

// DOM elements
let btnAuthenticate, btnGetSuppliers, btnGetOverlaps;
let authStatus, syncStatus, asyncStatus, apiResults, loadingIndicator;

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    initializeElements();
    setupEventHandlers();
    console.log('Exercise 3: Client application initialized');
});

function initializeElements() {
    // Get DOM elements
    btnAuthenticate = document.getElementById('btnAuthenticate');
    btnGetSuppliers = document.getElementById('btnGetSuppliers');
    btnGetOverlaps = document.getElementById('btnGetOverlaps');
    authStatus = document.getElementById('authStatus');
    syncStatus = document.getElementById('syncStatus');
    asyncStatus = document.getElementById('asyncStatus');
    apiResults = document.getElementById('apiResults');
    loadingIndicator = document.getElementById('loadingIndicator');
}

function setupEventHandlers() {
    // Authentication button
    btnAuthenticate.addEventListener('click', authenticateUser);
    
    // Exercise 3 Requirement 3: Synchronous API call for suppliers
    btnGetSuppliers.addEventListener('click', getSuppliersSync);
    
    // Exercise 3 Requirement 4: Asynchronous API call for overlaps
    btnGetOverlaps.addEventListener('click', getOverlapsAsync);
}

// Authentication function
function authenticateUser() {
    console.log('🔐 Authenticating user...');
    updateAuthStatus('Authenticating...', 'info');
    btnAuthenticate.disabled = true;

    // Use XMLHttpRequest for authentication
    const xhr = new XMLHttpRequest();
    xhr.open('POST', `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.auth}`, false); // Synchronous for auth
    xhr.setRequestHeader('Content-Type', 'application/json');
    
    xhr.onload = function() {
        if (xhr.status === 200) {
            const response = JSON.parse(xhr.responseText);
            jwtToken = response.token;
            updateAuthStatus('✅ Authenticated successfully!', 'success');
            enableApiButtons();
            console.log('🔐 JWT Token received:', jwtToken.substring(0, 50) + '...');
        } else {
            updateAuthStatus('❌ Authentication failed', 'danger');
            console.error('Authentication failed:', xhr.statusText);
        }
        btnAuthenticate.disabled = false;
    };

    xhr.onerror = function() {
        updateAuthStatus('❌ Authentication error', 'danger');
        btnAuthenticate.disabled = false;
        console.error('Authentication error:', xhr.statusText);
    };

    const authData = {
        username: API_CONFIG.credentials.username,
        password: API_CONFIG.credentials.password
    };

    xhr.send(JSON.stringify(authData));
}

// Exercise 3 Requirement 3: SYNCHRONOUS API call for suppliers and rates
function getSuppliersSync() {
    console.log('📊 Exercise 3 Requirement 3: Getting suppliers (SYNCHRONOUS)');
    
    if (!jwtToken) {
        alert('Please authenticate first!');
        return;
    }

    // Update UI to show synchronous behavior
    updateSyncStatus('🔄 Making synchronous call... (UI will freeze)', 'warning');
    btnGetSuppliers.disabled = true;
    showLoading(true);

    // Simulate the blocking behavior to demonstrate synchronous call
    setTimeout(() => {
        // SYNCHRONOUS XMLHttpRequest - this will block the UI thread
        const xhr = new XMLHttpRequest();
        xhr.open('GET', `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.suppliers}`, false); // false = synchronous
        xhr.setRequestHeader('Authorization', `Bearer ${jwtToken}`);
        
        try {
            xhr.send();
            
            if (xhr.status === 200) {
                const suppliers = JSON.parse(xhr.responseText);
                displaySuppliersData(suppliers);
                updateSyncStatus('✅ Synchronous call completed!', 'success');
                console.log('📊 Suppliers data received (sync):', suppliers.length, 'suppliers');
            } else {
                updateSyncStatus('❌ Synchronous call failed', 'danger');
                displayError('Failed to fetch suppliers: ' + xhr.statusText);
                console.error('Synchronous call failed:', xhr.status, xhr.statusText);
            }
        } catch (error) {
            updateSyncStatus('❌ Synchronous call error', 'danger');
            displayError('Error: ' + error.message);
            console.error('Synchronous call error:', error);
        }
        
        btnGetSuppliers.disabled = false;
        showLoading(false);
    }, 100); // Small delay to show the UI update first
}

// Exercise 3 Requirement 4: ASYNCHRONOUS API call for overlapping rates
async function getOverlapsAsync() {
    console.log('⚡ Exercise 3 Requirement 4: Getting overlaps (ASYNCHRONOUS)');
    
    if (!jwtToken) {
        alert('Please authenticate first!');
        return;
    }

    // Update UI to show asynchronous behavior
    updateAsyncStatus('🔄 Making asynchronous call... (UI remains responsive)', 'info');
    btnGetOverlaps.disabled = true;

    try {
        // ASYNCHRONOUS fetch API call - non-blocking
        const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.overlaps}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${jwtToken}`,
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const overlaps = await response.json();
            displayOverlapsData(overlaps);
            updateAsyncStatus('✅ Asynchronous call completed!', 'success');
            console.log('⚡ Overlaps data received (async):', overlaps.length, 'overlap groups');
        } else {
            updateAsyncStatus('❌ Asynchronous call failed', 'danger');
            displayError('Failed to fetch overlaps: ' + response.statusText);
            console.error('Asynchronous call failed:', response.status, response.statusText);
        }
    } catch (error) {
        updateAsyncStatus('❌ Asynchronous call error', 'danger');
        displayError('Error: ' + error.message);
        console.error('Asynchronous call error:', error);
    }

    btnGetOverlaps.disabled = false;
}

// Display suppliers data in user-friendly format
function displaySuppliersData(suppliers) {
    let html = `
        <div class="alert alert-info">
            <h5>📊 Suppliers and Rates Data (Retrieved Synchronously)</h5>
            <p><strong>Total Suppliers:</strong> ${suppliers.length}</p>
        </div>
    `;

    suppliers.forEach((supplier, index) => {
        html += `
            <div class="card mb-3">
                <div class="card-header bg-light">
                    <h6 class="mb-0">Supplier ${index + 1}: ${supplier.name}</h6>
                </div>
                <div class="card-body">
                    <p><strong>ID:</strong> ${supplier.supplierId}</p>
                    <p><strong>Address:</strong> ${supplier.address || 'N/A'}</p>
                    <p><strong>Created:</strong> ${new Date(supplier.createdOn).toLocaleDateString()}</p>
                    <p><strong>Created By:</strong> ${supplier.createdByUser}</p>
                    
                    <h6>Rates (${supplier.rates.length}):</h6>
                    <div class="table-responsive">
                        <table class="table table-sm table-striped">
                            <thead>
                                <tr>
                                    <th>Rate ID</th>
                                    <th>Amount</th>
                                    <th>Start Date</th>
                                    <th>End Date</th>
                                </tr>
                            </thead>
                            <tbody>
        `;

        supplier.rates.forEach(rate => {
            html += `
                <tr>
                    <td>${rate.supplierRateId}</td>
                    <td>$${rate.rate.toFixed(2)}</td>
                    <td>${new Date(rate.rateStartDate).toLocaleDateString()}</td>
                    <td>${rate.rateEndDate ? new Date(rate.rateEndDate).toLocaleDateString() : 'Ongoing'}</td>
                </tr>
            `;
        });

        html += `
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        `;
    });

    apiResults.innerHTML = html;
}

// Display overlapping rates data in user-friendly format
function displayOverlapsData(overlaps) {
    let html = `
        <div class="alert alert-warning">
            <h5>⚡ Overlapping Rates Data (Retrieved Asynchronously)</h5>
            <p><strong>Suppliers with Overlaps:</strong> ${overlaps.length}</p>
        </div>
    `;

    if (overlaps.length === 0) {
        html += `
            <div class="alert alert-success">
                <h6>✅ No Overlapping Rates Found</h6>
                <p>All supplier rates have proper date ranges without overlaps.</p>
            </div>
        `;
    } else {
        overlaps.forEach((supplierOverlap, index) => {
            html += `
                <div class="card mb-3 border-warning">
                    <div class="card-header bg-warning">
                        <h6 class="mb-0">⚠️ ${supplierOverlap.supplierName} (ID: ${supplierOverlap.supplierId})</h6>
                    </div>
                    <div class="card-body">
                        <p><strong>Number of Overlapping Rate Pairs:</strong> ${supplierOverlap.overlappingRates.length}</p>
            `;

            supplierOverlap.overlappingRates.forEach((overlap, overlapIndex) => {
                html += `
                    <div class="card mb-2 border-danger">
                        <div class="card-header bg-light">
                            <small><strong>Overlap ${overlapIndex + 1}</strong></small>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-5">
                                    <h6 class="text-primary">Rate 1</h6>
                                    <p><strong>Rate:</strong> $${overlap.rate1.rate.toFixed(2)}</p>
                                    <p><strong>Period:</strong> ${new Date(overlap.rate1.rateStartDate).toLocaleDateString()} - ${overlap.rate1.rateEndDate ? new Date(overlap.rate1.rateEndDate).toLocaleDateString() : 'Ongoing'}</p>
                                </div>
                                <div class="col-md-2 text-center">
                                    <div class="alert alert-danger p-2">
                                        <strong>OVERLAP</strong><br>
                                        <small>${overlap.overlapDays} days</small>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <h6 class="text-primary">Rate 2</h6>
                                    <p><strong>Rate:</strong> $${overlap.rate2.rate.toFixed(2)}</p>
                                    <p><strong>Period:</strong> ${new Date(overlap.rate2.rateStartDate).toLocaleDateString()} - ${overlap.rate2.rateEndDate ? new Date(overlap.rate2.rateEndDate).toLocaleDateString() : 'Ongoing'}</p>
                                </div>
                            </div>
                            <div class="alert alert-info p-2">
                                <strong>Overlap Period:</strong> ${new Date(overlap.overlapStartDate).toLocaleDateString()} - ${new Date(overlap.overlapEndDate).toLocaleDateString()}
                            </div>
                        </div>
                    </div>
                `;
            });

            html += `
                    </div>
                </div>
            `;
        });
    }

    apiResults.innerHTML = html;
}

// Utility functions
function updateAuthStatus(message, type) {
    authStatus.className = `alert alert-${type}`;
    authStatus.textContent = message;
}

function updateSyncStatus(message, type) {
    syncStatus.innerHTML = `<small class="text-${type === 'warning' ? 'warning' : type === 'success' ? 'success' : 'danger'}">${message}</small>`;
}

function updateAsyncStatus(message, type) {
    asyncStatus.innerHTML = `<small class="text-${type === 'info' ? 'info' : type === 'success' ? 'success' : 'danger'}">${message}</small>`;
}

function enableApiButtons() {
    btnGetSuppliers.disabled = false;
    btnGetOverlaps.disabled = false;
}

function showLoading(show) {
    loadingIndicator.style.display = show ? 'block' : 'none';
}

function displayError(message) {
    apiResults.innerHTML = `
        <div class="alert alert-danger">
            <h6>❌ Error</h6>
            <p>${message}</p>
        </div>
    `;
}

// Add some styles to demonstrate synchronous vs asynchronous behavior
document.addEventListener('DOMContentLoaded', function() {
    const style = document.createElement('style');
    style.textContent = `
        .demo-sync-freeze {
            pointer-events: none;
            opacity: 0.6;
        }
        
        .pulse {
            animation: pulse 1s infinite;
        }
        
        @keyframes pulse {
            0% { opacity: 1; }
            50% { opacity: 0.5; }
            100% { opacity: 1; }
        }
    `;
    document.head.appendChild(style);
});

console.log('🚀 Exercise 3 JavaScript loaded successfully');
console.log('📋 Requirements implemented:');
console.log('  ✅ Separate client project');
console.log('  ✅ Client-side scripting');
console.log('  ✅ Event-triggered API calls');
console.log('  ✅ Synchronous suppliers API call');
console.log('  ✅ Asynchronous overlaps API call');
console.log('  ✅ Data visible to user');
